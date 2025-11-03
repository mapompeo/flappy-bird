using UnityEngine;

public class PipeTrigger : MonoBehaviour
{
    private bool passed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (passed) return;

        if (collision.CompareTag("Player"))
        {
            passed = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }
        }
    }
}
