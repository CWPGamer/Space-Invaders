using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public Player player;

    public TMP_Text playerLivesText;
    public int playerScore {get; private set;}
    public TMP_Text playerScoreText;
    public int highScore {get; private set;}
    public TMP_Text highScoreText;
    public TMP_Text gameStateText;
    public GameObject gameOverScreen;
    public GameObject newScoreText;
    public GameObject pausePanel;

    private bool _pauseGame;
    private bool _gameActive;

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

    // Start is called before the first frame update
    void Start()
    {
        _gameActive = true;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        newScoreText.SetActive(false);
        gameOverScreen.SetActive(false);
        LoadHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameActive)
        {
            if (Input.GetButtonDown("Pause") && !_pauseGame)
            {
                PauseGame();
            }
            else if (Input.GetButtonDown("Pause") && _pauseGame)
            {
                ResumeGame();
            } 
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        _pauseGame = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        _pauseGame = false;
    }

    public void GameOver()
    {
        _gameActive = false;
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
        AudioManager.Instance.StopMusic();

        if (Enemy.enemyCount == 0)
        {
            gameStateText.text = "YOU WON!!!";
            AudioManager.Instance.PlayWinSFX();
        }
        else if (player.PlayerDied || Earth.touchedEarth)
        {
            gameStateText.text = "YOU LOSE...";
            AudioManager.Instance.PlayLoseSFX();
        }

        if (playerScore > highScore)
        {
            highScore = playerScore;
            highScoreText.text = highScore.ToString();
            newScoreText.SetActive(true);
            SaveHighScore();
        }
    }

    public bool IsGamePaused(){ return _pauseGame; }

    public void PlayerScores(int score)
    {
        SetPlayerScore(playerScore + score);
    }

    public void SetPlayerHealth(int hp)
    {
        playerLivesText.text = hp.ToString();
    }
    private void SetPlayerScore(int score)
    {
        playerScore = score;
        playerScoreText.text = playerScore.ToString();
        if (Enemy.enemyCount <= 0)
        {
            GameOver();
        }
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = highScore.ToString();
    }   
}
