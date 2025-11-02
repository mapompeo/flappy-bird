using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void gotoGameplay()
    {
        SceneManager.LoadScene("gameplay");

    }
}