using System;
using UnityEngine;

public class CanoFlappyBirdItj : MonoBehaviour
{
    [SerializeField] private float velocidadeCano = 5f;
    private void Update()
    {
        Move();
        if(transform.position.x < -8f) Destroy(gameObject);
    }

    private void Move()
    {
        transform.Translate(Time.deltaTime * velocidadeCano * Vector2.left);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var bird = other.gameObject.GetComponent<FlappyBirdPlayerItj>();
        if (bird == null) return;

        bird.GameOver();
    }
}
