using System;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private float pipeSpeed = 4f;
    private void Update()
    {
        Move();
        if(transform.position.x < -8f) Destroy(gameObject);
    }

    private void Move()
    {
        transform.Translate(Time.deltaTime * pipeSpeed * Vector2.left);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var bird = other.gameObject.GetComponent<Bird>();
        if (bird == null) return;

        bird.GameOver();
    }
}
