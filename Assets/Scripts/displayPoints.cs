using UnityEngine;
using TMPro; 

public class displayPoints : MonoBehaviour
{
    private TMP_Text textComponent;
    
    // variavel pra guardar qual foi o ultimo valor que a gente mostrou na tela
    private int ultimoSaldoVisto = -1; 

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // segurança basica
        if (textComponent == null) return;

        // pega quanto ta salvo na memoria agora
        int saldoAtual = PlayerPrefs.GetInt("PlayerPoints", 0);

        // O TRUQUE: so roda o codigo de texto se o numero mudou
        if (saldoAtual != ultimoSaldoVisto)
        {
            textComponent.text = "Pontos: " + saldoAtual.ToString();
            
            // atualiza nossa memoria pro proximo frame saber que ja mudamos
            ultimoSaldoVisto = saldoAtual;
        }
    }
}