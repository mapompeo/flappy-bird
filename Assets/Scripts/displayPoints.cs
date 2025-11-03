using UnityEngine;
using TMPro; 

public class DisplayPoints : MonoBehaviour
{
    private TMP_Text textComponent;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        
        int saldo = PlayerPrefs.GetInt("PlayerPoints", 0);
        
        textComponent.text = saldo.ToString();
        Debug.Log("Teste");
    }
}