using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Config")]
    [SerializeField] private GameConfig config;

    [Header("UI")]
    [SerializeField] private GameObject gameOverUI;

    public float ScrollSpeed { get; private set; }
    public float Distance { get; private set; }

    public bool IsGameOver { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ScrollSpeed = config.startSpeed;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        // Stop all movement/progression if game is over
        if (IsGameOver)
            return;

        ScrollSpeed = Mathf.Min(
            ScrollSpeed + config.speedIncreaseRate * Time.deltaTime,
            config.maxSpeed
        );

        Distance += ScrollSpeed * Time.deltaTime;
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        // Optional: freeze physics/time completely
        Time.timeScale = 0f;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}