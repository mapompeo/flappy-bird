using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BirdController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1.2f;
    }

    void Update()
    {
        // Pular com espaço ou clique
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // Movimento lateral
        Move();
    }

    void Jump()
    {
        // Zera a velocidade vertical antes de pular
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        // Aplica for�a para cima
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Move()
    {
        // Pega input horizontal (A/D ou Setas)
        float horizontal = Input.GetAxis("Horizontal");

        // Move o personagem mantendo a velocidade vertical
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }
}