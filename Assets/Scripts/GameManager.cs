using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject startPanel;

    private int score = 0;
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

        Time.timeScale = 0f; // jogo parado no início

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (startPanel != null)
            startPanel.SetActive(true); // mostra tela inicial

        UpdateUI();
    }

    private void Update()
    {
        // inicia ao apertar espaço ou clicar
        if (!gameStarted && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        Time.timeScale = 1f; // solta o jogo

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
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("Game Over! Score final: " + score);
    }

    public void ResetScore()
    {
        score = 0;
        isGameOver = false;
        gameStarted = false;
        Time.timeScale = 0f; // volta parado com startPanel

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (startPanel != null)
            startPanel.SetActive(true);

        UpdateUI();
    }
}
