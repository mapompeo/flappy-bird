using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
    // variaveis que aparecem no inspector pra gente configurar
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private Transform teto;

    [Header("Áudio de Morte")]
    [SerializeField] private AudioClip deathSFX;

    [Header("Áudio de Pulo")]
    [SerializeField] public AudioClip jumpSFX;
    [SerializeField] public float jumpSFXVolume = 1f;

    // variaveis privadas que o script usa pra funcionar
    private AudioSource audioSource;
    private Rigidbody2D _rb2D;
    private bool isDead = false;
    private Vector3 initialPosition;
    
    private Animator animator;
    
    // essa variavel serve pra saber se o jogador ja clicou a primeira vez
    private bool jogoComecou = false; 

    // essa funcao roda uma unica vez quando o objeto nasce na cena
    private void Awake()
    {
        // pega os componentes necessarios do objeto
        _rb2D = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
        
        // pega qual passaro foi salvo na memoria. se nao tiver nada, pega o 0
        int selectedID = PlayerPrefs.GetInt("PersonagemEscolhido", 0);

        // avisa o animator qual o id atual pra ele saber as transicoes
        animator.SetInteger("CharacterID", selectedID);
        
        // logica pra descobrir o nome exato da animacao (Bird01Animation, Bird02Animation, etc)
        // somamos +1 pq o id comeca em 0 mas suas animacoes comecam em 01
        int numeroDaAnimacao = selectedID + 1;
        string nomeDaAnimacao = "Bird0" + numeroDaAnimacao + "Animation";

        // forca o animator a tocar essa animacao agora mesmo, sem esperar transicao
        // o "-1" e padrao e o "0f" manda comecar do primeiro frame
        animator.Play(nomeDaAnimacao, -1, 0f);
        
        // permite que a animacao rode mesmo se o jogo estiver pausado (timescale 0)
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        
        // atualiza o visual do sprite instantaneamente pra nao piscar o passaro errado
        animator.Update(0f);
        
        // congela a animacao definindo a velocidade dela como zero
        animator.speed = 0f;

        // configura o som
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }
    
    // essa funcao roda todo santo frame do jogo
    private void Update()
    {
        // se morreu, nao faz mais nada
        if (isDead) return;
        
        // verifica se apertou pra pular
        Pular();
        
        // verifica se bateu no teto
        SubiuDemais();
    }

    // verifica se o passaro passou da altura do teto
    private void SubiuDemais()
    {
        if (transform.position.y > teto.position.y)
            Die();
    }

    // cuida da logica do pulo e do inicio do jogo
    private void Pular()
    {
        // se apertou espaco ou clicou com o mouse
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // se o jogo ainda nao tinha comecado, a gente solta a animacao agora
            if (!jogoComecou)
            {
                animator.speed = 1f; // velocidade volta ao normal (1)
                jogoComecou = true;  // marca que o jogo esta rodando
            }

            // zera a velocidade atual pra nao acumular forca e aplica o pulo pra cima
            _rb2D.velocity = Vector2.zero;
            _rb2D.velocity = Vector2.up * jumpSpeed;

            // toca o som de pulo se tiver audio configurado
            if (jumpSFX != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpSFX, jumpSFXVolume);
            }
        }
    }

    // essa funcao roda sozinha quando o passaro encosta em algo com colisor
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    // funcao pra matar o passaro
    public void Die()
    {
        // se ja ta morto, ignora pra nao morrer duas vezes
        if (isDead) return;
        isDead = true;

        // desliga esse script pra ele parar de pular
        this.enabled = false;

        // opcional: volta o animator pro tempo normal pra ele parar se o jogo pausar na tela de game over
        if(animator != null) animator.updateMode = AnimatorUpdateMode.Normal;

        // toca som de morte
        if (deathSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSFX);
        }

        // avisa o gerenciador do jogo que deu game over
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }

    // funcao pra resetar o passaro quando o jogo reinicia
    public void ResetBird()
    {
        isDead = false;
        jogoComecou = false; // reseta o estado pra ele congelar de novo
        this.enabled = true;

        // volta pra posicao inicial e zera a fisica
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        _rb2D.velocity = Vector2.zero;
        _rb2D.angularVelocity = 0f;
        _rb2D.isKinematic = false;
        
        // garante que o animator continue funcionando no reset
        if(animator != null) {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            
            // recarrega a skin correta e congela de novo
            int selectedID = PlayerPrefs.GetInt("PersonagemEscolhido", 0);
            int numeroDaAnimacao = selectedID + 1;
            string nomeDaAnimacao = "Bird0" + numeroDaAnimacao + "Animation";
            
            animator.Play(nomeDaAnimacao, -1, 0f);
            animator.speed = 0f; // congela novamente
        }
    }
}