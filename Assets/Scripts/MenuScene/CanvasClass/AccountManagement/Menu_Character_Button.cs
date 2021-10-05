using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Character_Button : MonoBehaviour
{
    private CharactersPanel charPanel;

    private Button button;

    private Image characterImage;

    private Character character;

    public Hero hero; // in case when button meant for hero;

    private Utility.UI_Char_Button ui_Char_Button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    public void Init(CharactersPanel charPanel, Character character, Utility.UI_Char_Button ui_Char_Button)
    {
        this.charPanel = charPanel;
        this.character = character;
        this.characterImage = transform.Find("Character_Image").GetComponent<Image>();
        
        this.ui_Char_Button = ui_Char_Button;

        if (hero.name != "") transform.Find("Text").GetComponent<Text>().text = hero.name;
        else transform.Find("Text").GetComponent<Text>().text = character.name;

        characterImage.sprite = character.image;
    }

    public void TaskOnClick()
    {
        Enable_Neighbours();

        if(button == null) button = GetComponent<Button>();
        button.interactable = false;

        switch(ui_Char_Button)
        {
            case Utility.UI_Char_Button.hero :
                string heroInfo = character.name + "\n";
                heroInfo += "Health: " + character.health.hp_cur + "/" + character.health.hp_max;
                charPanel.selected_hero_Text.text = heroInfo;

                charPanel.selected_hero_id = Get_Id_SelectedCharacter();
                charPanel.Update_HeroCharactersView();
            break;

            case Utility.UI_Char_Button.battleChar :
                string bCharInfo = character.name + "\n";
                bCharInfo += "Health: " + character.health.hp_cur + "/" + character.health.hp_max;
                charPanel.selected_hCharacter_Text.text = bCharInfo;

                charPanel.selected_hCharacter_id = Get_Id_SelectedCharacter();
            break;

            case Utility.UI_Char_Button.accChar :
                string charInfo = character.name + "\n";
                charInfo += "Health: " + character.health.hp_cur + "/" + character.health.hp_max;
                charPanel.selected_pCharacter_Text.text = charInfo;

                charPanel.selected_pCharacter_id = Get_Id_SelectedCharacter();
            break;
        }
	}

    private int Get_Id_SelectedCharacter()
    {
        Account acc = GameData.inst.account;
        int result = 0;

        switch(ui_Char_Button)
        {
            case Utility.UI_Char_Button.hero :
                for(int x = 0; x < acc.heroes.Count; x++)
                    if(acc.heroes[x].character == character) result = x;
            break;

            case Utility.UI_Char_Button.battleChar :
                Hero h = acc.heroes[charPanel.selected_hero_id];
                for(int x = 0; x < h.battleCharacters.Count; x++)
                    if(h.battleCharacters[x] == character) result = x;
            break;

            case Utility.UI_Char_Button.accChar :
                for(int x = 0; x < acc.сharacters.Count; x++)
                    if(acc.сharacters[x] == character) result = x;
            break;
        }

        return result;
    }

    private void Enable_Neighbours()
    {
        Transform content = transform.parent;
        foreach (Transform child in content)
            child.gameObject.GetComponent<Button>().interactable = true;
    }
}
