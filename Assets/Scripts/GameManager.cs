using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text liveScoreText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject newBestIcon;
    [SerializeField] private GameObject startPanel;

    private int score = 0;
    private int bestScore = 0;
    private bool isGameOver = false;
    private bool gameStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Carrega o recorde salvo
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (startPanel != null)
            startPanel.SetActive(true);

        UpdateUI();
    }

    private void Update()
    {
        if (!gameStarted && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            StartGame();
    }

    private void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f;

        if (startPanel != null)
            startPanel.SetActive(false);
    }

    public void AddScore(int points)
    {
        if (isGameOver || !gameStarted) return;

        score += points;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (liveScoreText != null)
            liveScoreText.text = score.ToString();

        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        newBestIcon.SetActive(false);
        if (score > bestScore)
        {
            newBestIcon.SetActive(true);
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore); // Salva a variavel no cache
            PlayerPrefs.Save();
        }

        if (scoreText != null)
            scoreText.text = score.ToString();

        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void ResetScore()
    {
        score = 0;
        isGameOver = false;
        gameStarted = false;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (startPanel != null)
            startPanel.SetActive(true);

        UpdateUI();
    }
}
