using UnityEngine;
using System.Collections; 

public class selectPlayer : MonoBehaviour
{
    private const string CharacterPrefKey = "PersonagemEscolhido";
    private const string PointsKey = "PlayerPoints";

    [Header("Arraste o objeto de Texto (Pontos) aqui")]
    public Transform textoParaTremer; 

    // variavel pra controlar se o texto ja ta sacudindo
    private bool estaTremendo = false;

    // nova variavel pra guardar a posicao fixa do texto
    private Vector3 posicaoInicialFixa;

    // usamos o start pra salvar a posicao do texto assim que o jogo abre
    void Start()
    {
        if (textoParaTremer != null)
        {
            posicaoInicialFixa = textoParaTremer.localPosition;
        }
    }

    // botao 1: gratis
    public void selectChar01()
    {
        Debug.Log("Personagem 0 (Grátis) selecionado!");
        PlayerPrefs.SetInt(CharacterPrefKey, 0);
        PlayerPrefs.Save();
    }

    // botao 2: precisa de 25 pontos
    public void selectChar02()
    {
        int saldo = PlayerPrefs.GetInt(PointsKey, 0);

        if (saldo >= 25)
        {
            Debug.Log("Personagem 1 selecionado!");
            PlayerPrefs.SetInt(CharacterPrefKey, 1);
            PlayerPrefs.Save();
        }
        else
        {
            StartCoroutine(TremerObjeto());
        }
    }

    // botao 3: precisa de 50 pontos
    public void selectChar03()
    {
        int saldo = PlayerPrefs.GetInt(PointsKey, 0);

        if (saldo >= 50)
        {
            Debug.Log("Personagem 2 selecionado!");
            PlayerPrefs.SetInt(CharacterPrefKey, 2);
            PlayerPrefs.Save();
        }
        else
        {
            StartCoroutine(TremerObjeto());
        }
    }

    // botao 4: precisa de 100 pontos
    public void selectChar04()
    {
        int saldo = PlayerPrefs.GetInt(PointsKey, 0);

        if (saldo >= 100)
        {
            Debug.Log("Personagem 3 selecionado!");
            PlayerPrefs.SetInt(CharacterPrefKey, 3);
            PlayerPrefs.Save();
        }
        else
        {
            StartCoroutine(TremerObjeto());
        }
    }

    // --- logica do tremor ---
    IEnumerator TremerObjeto()
    {
        if (textoParaTremer == null || estaTremendo == true) 
        {
            yield break;
        }

        estaTremendo = true;

        // mantive sua lógica de força, pode ajustar se quiser
        float forca = 0.12f; 

        for (int i = 0; i < 5; i++)
        {
            Vector3 novaPosicao = posicaoInicialFixa + (Vector3)(Random.insideUnitCircle * forca);
            textoParaTremer.localPosition = novaPosicao;
            
            // --- A MUDANÇA É AQUI ---
            // WaitForSeconds trava se o jogo tiver pausado (TimeScale 0)
            // WaitForSecondsRealtime funciona sempre, mesmo com o jogo pausado
            yield return new WaitForSecondsRealtime(0.05f);
        }

        textoParaTremer.localPosition = posicaoInicialFixa;
        estaTremendo = false;
    }
}