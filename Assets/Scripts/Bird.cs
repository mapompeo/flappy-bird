using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private Transform teto;

    private Rigidbody2D _rb2D;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Pular();
        SubiuDemais();
    }

    private void SubiuDemais()
    {
        if (transform.position.y > teto.position.y)
            GameOver();
    }

    private void Pular()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        _rb2D.velocity = Vector2.up * jumpSpeed;
    }

    // Detecta colisão com canos e chão
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameOver();
    }

    public void GameOver()
    {
        // Chama o Game Over do GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }
}