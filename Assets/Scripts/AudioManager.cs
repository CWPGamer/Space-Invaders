using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Music
    public AudioSource musicSource;
    public AudioClip[] spaceMusic;
    private int _musicIndex = 0;
    [SerializeField] private float cycleSpeed = 2f;

    // SFX
    public AudioSource sfxSource;
    public AudioClip lose;
    public AudioClip win;
    public AudioClip enemyHurt;
    public AudioClip playerHurt;
    public AudioClip playerShoot;
    public AudioClip enemyShoot;

    public bool once = false;

    public static AudioManager Instance {get; private set;}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }        
    }

    void Start()
    {
        StartCoroutine("PlayMusic");
    }

    private void Update()
    {
        if (once)
        {
            if (Enemy.moveCount % 1 == 0 && Enemy.moveCount != 0)
            {
                once = false;
                IncreaseMusicSpeed();
            } 
        }
    }
    

    IEnumerator PlayMusic()
    {
        while(true)
        {
            musicSource.clip = spaceMusic[_musicIndex];
            musicSource.Play();
            yield return new WaitForSeconds(cycleSpeed);
            _musicIndex = (_musicIndex + 1) % spaceMusic.Length;
        }
    }

    public void StopMusic()
    {
        StopCoroutine("PlayMusic");
    }

    public void IncreaseMusicSpeed()
    {
        cycleSpeed = cycleSpeed > 0.2f ? cycleSpeed - 0.1f : 0.1f;
    }

    public void PlayPlayerHurt()
    {
        sfxSource.PlayOneShot(playerHurt);
    }

    public void PlayShoot()
    {
        sfxSource.PlayOneShot(playerShoot);
    }
    public void PlayEnemyShoot()
    {
        sfxSource.PlayOneShot(enemyShoot);
    }

    public void PlayDestroyEnemy()
    {
        sfxSource.PlayOneShot(enemyHurt);
    }

    public void PlayLoseSFX()
    {
        sfxSource.PlayOneShot(lose);
    }
    
    public void PlayWinSFX()
    {
        sfxSource.PlayOneShot(win);
    }
}
