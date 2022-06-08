using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB;
    public Text nameText;
    public Sprite headSprite;
    public SpriteRenderer artworkSprite;
    public Text Text;

    public static string pseudo;
    public static string race;
    private int selectedOption = 0;

    public GameObject panelSelection, panelPseudo;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }
        UpdateCharacter(selectedOption);
    }
    void Update()
    {
        pseudo = Text.text;
    }
    public void PlayGame()
    {
        Debug.Log($"CM pseudo : {pseudo}");
        SceneManager.LoadScene("Map");
    }

    public void NextOption()
    {
        selectedOption++; 
        if (selectedOption >= characterDB.CharacterCount)
        {
            selectedOption = 0;
        }

        UpdateCharacter(selectedOption);
        Save();
    }

    public void BackOption()
    {
        selectedOption--; 
        if (selectedOption < 0)
        {
            selectedOption = characterDB.CharacterCount -1;
        }

        UpdateCharacter(selectedOption);
        Save();
    }

    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        headSprite = character.headSprite;
        artworkSprite.sprite = character.characterSprite;
        nameText.text = character.characterName;
        race = character.characterName;
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
    private void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
        PlayerPrefs.SetString("pseudo", pseudo);
    }

    public void Accept()
    {
        panelSelection.SetActive(false);
        panelPseudo.SetActive(true);
    }
}
