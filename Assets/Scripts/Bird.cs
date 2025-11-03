using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private Transform teto;

    [Header("Áudio de Morte")]
    [SerializeField] private AudioClip deathSFX;

    [Header("Áudio de Pulo")]
    [SerializeField] public AudioClip jumpSFX;
    [SerializeField] public float jumpSFXVolume = 1f;

  

    private AudioSource audioSource;
    private Rigidbody2D _rb2D;
    private bool isDead = false;
    private Vector3 initialPosition;
    
    private Animator animator;


    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        animator = GetComponent<Animator>();
        
        int selectedID = PlayerPrefs.GetInt("PersonagemEscolhido", 0);

        animator.SetInteger("CharacterID", selectedID);
        


        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    
    private void Update()
    {
        if (isDead) return;
        Pular();
        SubiuDemais();
    }

    private void SubiuDemais()
    {
        if (transform.position.y > teto.position.y)
            Die();
    }

    private void Pular()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            _rb2D.velocity = Vector2.zero;
            _rb2D.velocity = Vector2.up * jumpSpeed;

            if (jumpSFX != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpSFX, jumpSFXVolume);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        this.enabled = false;

        if (deathSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSFX);
        }

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }

    public void ResetBird()
    {
        isDead = false;
        this.enabled = true;

        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;

        _rb2D.velocity = Vector2.zero;
        _rb2D.angularVelocity = 0f;
        _rb2D.isKinematic = false;
    }
}