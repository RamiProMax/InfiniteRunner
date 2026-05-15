using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string sceneToLoad;

    [Header("UI")]
    [SerializeField] private TMP_Text highScoreText;

    private const string HIGH_SCORE_KEY = "HIGH_SCORE";

    void Start()
    {
        UpdateHighScoreUI();
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogWarning("Scene name is empty!");
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void UpdateHighScoreUI()
    {
        if (highScoreText == null) return;

        float highScore = PlayerPrefs.GetFloat(HIGH_SCORE_KEY, 0f);
        highScoreText.text = Mathf.FloorToInt(highScore).ToString();
    }
}