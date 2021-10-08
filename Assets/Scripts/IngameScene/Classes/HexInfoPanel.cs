using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexInfoPanel
{
    private SceneMain_UI sceneUI;

    private GameObject canvas_Go;
    private GameObject hexInfo_Go;
    private Image hex_Image;
    private Text hex_Text;

    private GameObject characterInfo_Go;
    private Image character_Image;
    private Text character_Text;

    private CharactersData cd;

    public HexInfoPanel(SceneMain_UI sceneUI)
    {
        this.sceneUI = sceneUI;
        this.canvas_Go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/HexInfo_Canvas", typeof(GameObject))) as GameObject;

        this.hexInfo_Go = canvas_Go.transform.Find("Hex_Panel").gameObject;
        this.hex_Image = hexInfo_Go.transform.Find("Hex_Image").GetComponent<Image>();
        this.hex_Text = hexInfo_Go.transform.Find("HexInfo_Text").GetComponent<Text>();

        this.characterInfo_Go = canvas_Go.transform.Find("Character_Panel").gameObject;
        this.character_Image = characterInfo_Go.transform.Find("Character_Image").GetComponent<Image>();
        this.character_Text = characterInfo_Go.transform.Find("CharacterInfo_Text").GetComponent<Text>();

        Hide_All();

        cd = new CharactersData();
    }

    public void Hide_All()
    {
        this.hexInfo_Go.SetActive(false);
        this.characterInfo_Go.SetActive(false);
    }

    public void Hide_HexInfo()
    {
        this.hexInfo_Go.SetActive(false);
    }

    public void Hide_CharacterInfo()
    {
        this.characterInfo_Go.SetActive(false);
    }

    public void Show_HexInfo(Hex hex)
    {
        this.hexInfo_Go.SetActive(true);

        string result = hex.tr.name;
        result += "\nMove cost : " + hex.moveCost;
        result += "\nDodge bonus" + hex.dodge;

        this.hex_Text.text = result;
    }

    public void Show_CharacterInfo(Character character)
    {
        if(character == null) return;

        this.characterInfo_Go.SetActive(true);

        this.character_Image.sprite = character.image;
        this.character_Text.text = cd.Get_Ingame_Character_Tooltip(character);
    }
}
