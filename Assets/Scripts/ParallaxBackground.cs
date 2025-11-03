using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    private float startPosX;
    private float spriteWidth;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        startPosX = transform.position.x;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void Update()
    {
        // Move o fundo para a esquerda
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Quando sair totalmente da tela, reposiciona no início
        if (transform.position.x < startPosX - spriteWidth)
        {
            transform.position = new Vector3(startPosX, transform.position.y, transform.position.z);
        }
    }
}
