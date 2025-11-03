using UnityEngine;

// Este script vai em UM objeto na sua cena Loja (ex: "ShopManager")
public class selectPlayer : MonoBehaviour
{
    // A "chave" que vamos usar para salvar. É bom definir como constante.
    private const string CharacterPrefKey = "PersonagemEscolhido";

    // Botão 1 vai chamar esta função
    public void selectChar01()
    {
        Debug.Log("Personagem 0 selecionado!");
        PlayerPrefs.SetInt(CharacterPrefKey, 0);
        PlayerPrefs.Save();
    }

    // Botão 2 vai chamar esta função
    public void selectChar02()
    {
        Debug.Log("Personagem 1 selecionado!");
        PlayerPrefs.SetInt(CharacterPrefKey, 1);
        PlayerPrefs.Save();
    }

    // Botão 3...
    public void selectChar03()
    {
        Debug.Log("Personagem 2 selecionado!");
        PlayerPrefs.SetInt(CharacterPrefKey, 2);
        PlayerPrefs.Save();
    }

    // Botão 4...
    public void selectChar04()
    {
        Debug.Log("Personagem 3 selecionado!");
        PlayerPrefs.SetInt(CharacterPrefKey, 3);
        PlayerPrefs.Save();
    }
}