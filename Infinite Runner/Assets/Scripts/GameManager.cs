using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Config")]
    [SerializeField] private GameConfig config;

    [Header("UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameEndScoreText;
    [SerializeField] private TMP_Text highScoreText;

    private const string HIGH_SCORE_KEY = "HIGH_SCORE";

    public float ScrollSpeed { get; private set; }
    public float Distance { get; private set; }

    public float HighScore { get; private set; }

    public bool IsGameOver { get; private set; }

    void Awake()
    {
        Time.timeScale = 1f;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ScrollSpeed = config.startSpeed;

        HighScore = PlayerPrefs.GetFloat(HIGH_SCORE_KEY, 0f);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (IsGameOver)
            return;

        ScrollSpeed = Mathf.Min(
            ScrollSpeed + config.speedIncreaseRate * Time.deltaTime,
            config.maxSpeed
        );

        Distance += ScrollSpeed * Time.deltaTime;

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = ": " + Mathf.FloorToInt(Distance);
        }
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        Time.timeScale = 0f;

        float finalScore = Mathf.FloorToInt(Distance);

        // Save high score
        if (finalScore > HighScore)
        {
            HighScore = finalScore;
            PlayerPrefs.SetFloat(HIGH_SCORE_KEY, HighScore);
            PlayerPrefs.Save();
        }

        // Game over UI updates
        if (gameEndScoreText != null)
            gameEndScoreText.text = "Score: " + finalScore;

        if (highScoreText != null)
            highScoreText.text = "High Score: " + HighScore;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }


    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}