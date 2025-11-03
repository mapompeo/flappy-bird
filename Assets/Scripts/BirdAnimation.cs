using UnityEngine;

public class BirdAnimation : MonoBehaviour
{
    [Header("Configuração de Rotação")]
    [SerializeField] private float rotationSpeed = 10f; 
    [SerializeField] private float maxUpRotation = 30f;  

    private Rigidbody2D birdRigidbody;
    private bool isDead = false; 

    public void SetDead(bool dead)
    {
        isDead = dead;
    }

    private void Awake()
    {
        birdRigidbody = GetComponent<Rigidbody2D>();
        if (birdRigidbody == null)
        {
            Debug.LogError("BirdAnimation requer um Rigidbody2D no mesmo GameObject.");
            enabled = false; // Desabilita o script se não encontrar o Rigidbody2D
        }
    }

    private void FixedUpdate()
    {
        if (birdRigidbody == null) return; 

        if (isDead)
        {
            RotateOnDeath();
        }
        else
        {
         
            RotateBasedOnVelocity();
        }
    }

    private void RotateBasedOnVelocity()
    {
        float rotationAngle;
        
        if (birdRigidbody.velocity.y > 0)
        {
            // Subindo: Pega o ângulo de acordo com a velocidade (limitado pelo maxUpRotation)
            rotationAngle = Mathf.Min(maxUpRotation, birdRigidbody.velocity.y * 5f); 
        }
        else
        {
            // Caindo: Inclina para baixo até -90 graus
            rotationAngle = Mathf.Max(-90f, birdRigidbody.velocity.y * 7f); 
        }

        // Cria a rotação alvo
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotationAngle);

        // Aplica a rotação de forma suave (Lerp)
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }
    
    private void RotateOnDeath()
    {
        // Força o nariz para baixo rapidamente em 90 graus
        Quaternion targetRotation = Quaternion.Euler(0, 0, -90f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed * 2f); // * 2f para ser mais rápido
    }
}