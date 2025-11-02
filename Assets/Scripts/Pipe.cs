using UnityEngine;

public class Pipe : MonoBehaviour
{
    private float pipeSpeed = 4f;
    private bool pointGiven = false;
    private Transform bird;

    private void Start()
    {
        bird = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void SetSpeed(float newSpeed)
    {
        pipeSpeed = newSpeed;
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * pipeSpeed * Vector2.left);

        // Marca ponto quando o pássaro passa o cano
        if (bird != null && !pointGiven && bird.position.x > transform.position.x)
        {
            pointGiven = true;
            GameManager.Instance.AddScore(1);
        }

        if (transform.position.x < -8f)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var bird = other.gameObject.GetComponent<Bird>();
        if (bird == null) return;

        bird.GameOver();
    }
}
