using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel; // <- NOVO

    private int score = 0;
    private bool isGameOver = false; // <- NOVO

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Garante que o Game Over est� escondido no in�cio
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateUI();
    }

    public void AddScore(int points)
    {
        if (isGameOver) return; // N�o adiciona pontos ap�s Game Over

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
        if (isGameOver) return; // Evita chamar m�ltiplas vezes

        isGameOver = true;

        // Mostra o painel de Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Pausa o jogo (opcional)
        Time.timeScale = 0f;

        Debug.Log("Game Over! Score final: " + score);
    }

    public void ResetScore()
    {
        score = 0;
        isGameOver = false;
        Time.timeScale = 1f; // Volta o tempo normal

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateUI();
    }
}