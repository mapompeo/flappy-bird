using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("==== REINICIANDO JOGO ===="); // Para testar

        Time.timeScale = 1f;

        // Reseta o GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.ResetScore();

        // Recarrega pelo NOME da cena (mais confiável)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}