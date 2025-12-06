using UnityEngine;

public class clearPoints : MonoBehaviour
{
    // funcao publica pra gente poder chamar atraves de um botao
    public void ZerarPontos()
    {
        // acessa a memoria e força o valor da chave "PlayerPoints" virar 0
        PlayerPrefs.SetInt("PlayerPoints", 0);

        // obriga a unity a salvar essa alteracao no disco agora mesmo
        PlayerPrefs.Save();

        // so um aviso no console pra voce ter certeza que o clique funcionou
        Debug.Log("os pontos foram zerados");
    }
}