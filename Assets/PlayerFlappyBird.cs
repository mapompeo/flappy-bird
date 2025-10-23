using UnityEngine;

public class PlayerFlappyBird : MonoBehaviour
{
    public static PlayerFlappyBird Instance;
    
    private Rigidbody2D _rb2D;
    [SerializeField] private float velocidadePulo = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Pular();
    }

    private void Pular()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, velocidadePulo);
    }
    public void GameOver()
    {
        Debug.Log("Game Over");
    }
}
