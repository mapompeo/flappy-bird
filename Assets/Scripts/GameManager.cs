using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    
    [SerializeField] private Bird birdScript; 

    // --- ÁUDIO ---

    [Header("Áudio de Morte")]
    [SerializeField] private AudioClip deathSFX; // sfx_hit
    [SerializeField] private float deathSFXVolume = 1.0f; 

    [Header("Áudio de Pontuação")]
    [SerializeField] private AudioClip pointSFX; // sfx_point
    [SerializeField] private float pointSFXVolume = 0.05f; 
   
    private int score = 0;
    private bool isGameOver = false;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; 
        audioSource.volume = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateUI();
    }
    
    public void AddScore(int points)
    {
        if (isGameOver) return;

        score += points;
        UpdateUI();

        // toca som de ponto
        if (pointSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(pointSFX, pointSFXVolume); 
        }
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

        // mostra painel de Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // pausa o jogo
        Time.timeScale = 0f;
    }

    public void ResetScore()
    {
        score = 0;
        isGameOver = false;

        if (birdScript != null)
        {
            birdScript.ResetBird(); 
        }
        
        Time.timeScale = 1f; 

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateUI();
    }
}