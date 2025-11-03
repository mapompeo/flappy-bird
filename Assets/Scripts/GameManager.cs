using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject startPanel;
    [SerializeField] private TMP_Text liveScoreText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestScoreText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject newBestIcon;
    [SerializeField] private Image medalImage;
    [SerializeField] private Sprite bronzeMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite platinumMedal;

    [SerializeField] private Bird birdScript;

    [Header("UI")]
    [SerializeField] private Animator gameOverAnimator;
    [SerializeField] private Image flashPanel;

    [Header("Áudio de Morte")]
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private float deathSFXVolume = 1.0f;

    [Header("Áudio de Pontuação")]
    [SerializeField] private AudioClip pointSFX;
    [SerializeField] private float pointSFXVolume = 0.05f;

    private int score = 0;
    private int bestScore = 0;
    private bool isGameOver = false;
    private bool gameStarted = false;
    private AudioSource audioSource;

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

        // Configura AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 1f;

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

        if (pointSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(pointSFX, pointSFXVolume);
        }
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

        // Flash branco
        if (flashPanel != null)
        {
            StartCoroutine(FlashEffect());
        }

        // SFX de morte
        if (deathSFX != null)
        {
            GameObject tempAudio = new GameObject("TempDeathSFX");
            AudioSource deathAudio = tempAudio.AddComponent<AudioSource>();
            deathAudio.clip = deathSFX;
            deathAudio.playOnAwake = false;
            deathAudio.loop = false;
            deathAudio.spatialBlend = 0f;
            deathAudio.volume = deathSFXVolume;
            deathAudio.Play();
            Destroy(tempAudio, deathSFX.length);
        }

        // --- ALTERAÇÃO 1 INICIA AQUI ---
        // Ativa o painel PRIMEIRO, para a animação poder tocar
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        // --- ALTERAÇÃO 1 TERMINA AQUI ---
        

        // Animação do Game Over
        if (gameOverAnimator != null)
        {
            CanvasGroup cg = gameOverAnimator.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
            gameOverAnimator.SetTrigger("Show"); // <-- Agora funciona, pois o painel está ativo
        }
        else
        {
            Debug.LogError("O 'gameOverAnimator' não foi definido no Inspector do GameManager!");
        }

        // Medalhas e recorde
        newBestIcon.SetActive(false);
        if (score > bestScore)
        {
            newBestIcon.SetActive(true);
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }
        
        // Atualiza o saldo de pontos
        int saldoAntigo = PlayerPrefs.GetInt("PlayerPoints", 0);
        int novoSaldo = saldoAntigo + score;
        PlayerPrefs.SetInt("PlayerPoints", novoSaldo);
        PlayerPrefs.Save();
        

        medalImage.gameObject.SetActive(true);
        if (score >= 40)
            medalImage.sprite = platinumMedal;
        else if (score >= 30)
            medalImage.sprite = goldMedal;
        else if (score >= 20)
            medalImage.sprite = silverMedal;
        else if (score >= 10)
            medalImage.sprite = bronzeMedal;
        else
        {
            medalImage.sprite = null;
            medalImage.gameObject.SetActive(false);
        }

        // Atualiza textos finais
        if (scoreText != null)
            scoreText.text = score.ToString();
        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();

        // (Linha 'gameOverPanel.SetActive(true)' foi REMOVIDA DAQUI pois foi movida para cima)

        Time.timeScale = 0f;
    }
    
    private IEnumerator FlashEffect()
    {
        float fadeDuration = 0.30f;
        float startAlpha = 0.8f;
        Color flashColor = flashPanel.color;
        flashColor.a = startAlpha;
        flashPanel.color = flashColor;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            flashColor.a = Mathf.Lerp(startAlpha, 0, timer / fadeDuration);
            flashPanel.color = flashColor;
            yield return null;
        }

        flashColor.a = 0;
        flashPanel.color = flashColor;
    }

    public void ResetScore()
    {
        score = 0;
        isGameOver = false;
        gameStarted = false;
        Time.timeScale = 0f;

        if (birdScript != null)
            birdScript.ResetBird();

        // --- ALTERAÇÃO 2 INICIA AQUI ---
        // 1. Reseta a animação PRIMEIRO (enquanto o painel ainda está visível)
        if (gameOverAnimator != null)
        {
            CanvasGroup cg = gameOverAnimator.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
            gameOverAnimator.Play("Hidden", 0, 0f); // <-- Agora funciona
        }
        
        // 2. AGORA desativa o painel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        // --- ALTERAÇÃO 2 TERMINA AQUI ---

        if (startPanel != null)
            startPanel.SetActive(true);

        // (Blocos 'gameOverPanel.SetActive(false)' e 'gameOverAnimator' foram REMOVIDOS DAQUI pois foram movidos para cima)

        UpdateUI();
    }
}