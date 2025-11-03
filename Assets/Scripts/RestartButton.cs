using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
	[SerializeField] private GameObject gameOverPanel;
    public void RestartGame()
    {
        Debug.Log("==== REINICIANDO JOGO ===="); // Para testar

        Time.timeScale = 1f;

        // Reseta o GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.ResetScore();

        // Recarrega pelo NOME da cena (mais confi�vel)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}