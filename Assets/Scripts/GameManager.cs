using UnityEngine;
using TMPro;
using System.Collections; // NECESSÁRIO PARA COROUTINES
using UnityEngine.UI;    // NECESSÁRIO PARA O COMPONENTE 'IMAGE'

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
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

        UpdateUI();
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;
        score += points;
        UpdateUI();
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

    
        if (flashPanel != null)
        {
            StartCoroutine(FlashEffect());
        }

 
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

        
        if (gameOverAnimator != null)
        {
            CanvasGroup cg = gameOverAnimator.GetComponent<CanvasGroup>();
            cg.interactable = true;
            cg.blocksRaycasts = true;
            gameOverAnimator.SetTrigger("Show");
        }
        else
        {
            Debug.LogError("O 'gameOverAnimator' não foi definido no Inspector do GameManager!");
        }

        Time.timeScale = 0f;
    }


    private IEnumerator FlashEffect()
    {
        float fadeDuration = 0.30f; // Duração do FlashEffect
        float startAlpha = 0.8f;    // Opacidade máxima do flashEffect
        Color flashColor = flashPanel.color;
        
       
        flashColor.a = startAlpha;
        flashPanel.color = flashColor;

        float timer = 0f;

     
        while (timer < fadeDuration)
        {
           
            timer += Time.unscaledDeltaTime; 
            
            // Interpola do alpha inicial (0.8) até 0
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

        if (birdScript != null)
        {
            birdScript.ResetBird();
        }

        Time.timeScale = 1f;
        
        if (gameOverAnimator != null)
        {
            CanvasGroup cg = gameOverAnimator.GetComponent<CanvasGroup>();
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            gameOverAnimator.Play("Hidden", 0, 0f);
        }

        UpdateUI();
    }
}