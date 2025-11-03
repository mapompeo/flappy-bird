using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1.2f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || TouchBegan())
        {
            Jump();
        }
    }

    bool TouchBegan()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    void Jump()
    {
        // Zera a velocidade vertical antes de pular
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        // Aplica força para cima
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
