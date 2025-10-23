using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This player script handles mechanics such as player movement,
    shooting, detecting input and collisions with enemy bullets and enemies.
*/

public class PlayerController : Player
{
    public BulletSpawner bulletSpawner;
    private Rigidbody2D rb;
    private float _xInput;
    [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _movement;

    [SerializeField] private float _attackCooldown = 0.75f;
    private float attackTimer = 0f;
    private float hitTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ResetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput && !GameManager.Instance.IsGamePaused())
        {
            ReadInput();
            _movement = new Vector2(_xInput * _moveSpeed, rb.velocity.y);
            rb.velocity = _movement;
        }
        attackTimer += Time.deltaTime;

        if (_invulnerable)
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= _invulnerableTime)
            {
                _invulnerable = false;
                hitTimer = 0;
            }
        }
    }

    void ReadInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal") * _moveSpeed;

        if (Input.GetButton("Shoot") && attackTimer >= _attackCooldown)
        {
            bulletSpawner.SpawnBullet();
            AudioManager.Instance.PlayShoot();
            attackTimer = 0;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet_Enemy") || collision.gameObject.CompareTag("Enemy"))
        {
            if (!_invulnerable)
            {
                TakeLife();
            }
        }
    }
}
