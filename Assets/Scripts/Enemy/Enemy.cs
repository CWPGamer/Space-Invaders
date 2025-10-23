using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static int enemyCount;

    public BulletSpawner bulletSpawner;

    private SpriteRenderer sr;
    public Sprite[] sprites;
    private int currentSpriteIndex = 0;
    [SerializeField] private float animationSpeed = 0.5f;

    [SerializeField] private int hitPoints = 1;
    public float _moveSpeed = 0.25f;
    public float _moveInterval = 2f;
    public int moveDistance = 5;
    public static int moveCount = 0;

    protected float _attackSpeed;
    public float _minAttackSpeed;
    public float _maxAttackSpeed;
    public int score;

    private bool _invulnerable = false;
    [SerializeField] private float _invulnerableTime = 1f;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (gameObject.name != "Mothership")
        {
            enemyCount++;
        }
        StartCoroutine("MoveEnemy");
        StartCoroutine("AnimateSprite");
        StartCoroutine("ShootBullet");
    }

    // Update is called once per frame
    void Update()
    {
        if (_invulnerable)
        {
            timer += Time.deltaTime;
            if (timer >= _invulnerableTime)
            {
                _invulnerable = false;
                timer = 0;
            }
        }
    }

    IEnumerator AnimateSprite()
    {
        while (true)
        {
            yield return new WaitForSeconds(animationSpeed);
            currentSpriteIndex = currentSpriteIndex == 0 ? 1 : 0;
            GetComponent<SpriteRenderer>().sprite = sprites[currentSpriteIndex];
        }
    }

    IEnumerator MoveEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(_moveInterval);

            for (int i = 0; i <= moveDistance; i++)
            {
                transform.position += Vector3.right * _moveSpeed;

                yield return new WaitForSeconds(_moveInterval);
            }


            if (gameObject.name != "Mothership")
            {
                transform.position += Vector3.down * Mathf.Abs(_moveSpeed);
            }

            moveCount++;
            AudioManager.Instance.once = true;
            IncreaseEnemySpeed();
            _moveSpeed *= -1;
        }
    }

    private void IncreaseEnemySpeed()
    {
        _moveInterval = _moveInterval > 0.125f ? _moveInterval - 0.125f : 0.125f;
        animationSpeed = animationSpeed > 0.0625f ? animationSpeed - 0.0625f : 0.0625f;
    }

    IEnumerator FlickerEnemy()
    {
        do
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.15f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.15f);
        } while (_invulnerable);
    }

    IEnumerator ShootBullet()
    {
        while (true)
        {
            _attackSpeed = Random.Range(_minAttackSpeed, _maxAttackSpeed);
            yield return new WaitForSeconds(_attackSpeed);
            bulletSpawner.SpawnBullet();
        }
    }

    void TakeLife()
    {
        hitPoints = hitPoints > 0 ? hitPoints - 1 : 0;
        if (hitPoints > 0)
        {
            _invulnerable = true;
            StartCoroutine("FlickerEnemy");
        }
        else if (hitPoints == 0)
        {
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        if (gameObject.name != "Mothership")
        {
            enemyCount--;
            //Debug.Log($"{enemyCount} Enemies Left");
        }
        GameManager.Instance.PlayerScores(score);
        AudioManager.Instance.PlayDestroyEnemy();
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet_Player") && !_invulnerable)
        {
            TakeLife();
        }
    }

    ~Enemy()
    {
        if (gameObject.name != "Mothership")
        {
            enemyCount--;
        }
    }
}
