using UnityEngine;
using UnityEngine.SceneManagement;

public class menuButton : MonoBehaviour
{
    public void gotoMenu()
    {
        SceneManager.LoadScene("menu");

    }
}