using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Character_Button : MonoBehaviour
{
    public PlayerManagementMenu pmm;
    private Button b;
    private Image characterImage;
    private Character character;
    private bool hero;
    private bool hCharacter;
    private bool pCharacter;

    private void Start()
    {
        b = GetComponent<Button>();
        b.onClick.AddListener(TaskOnClick);
    }

    public void Init(PlayerManagementMenu pmm, Character character, bool hero, bool hCharacter, bool pCharacter)
    {
        this.pmm = pmm;
        this.character = character;
        this.characterImage = transform.Find("Character_Image").GetComponent<Image>();
        this.hero = hero;
        this.hCharacter = hCharacter;
        this.pCharacter = pCharacter;

        if (hero) transform.Find("Text").GetComponent<Text>().text = character.cHero.name;
        else transform.Find("Text").GetComponent<Text>().text = character.cName;

        characterImage.sprite = character.cImage;
    }

    public void TaskOnClick()
    {
        Enable_Neighbours();

        if(b == null) GetComponent<Button>().interactable = false;
        else b.interactable = false;

		if(hero)
        {
            pmm.selected_hero_Name_Text.text = character.cHero.name;
            pmm.selected_hero_id = Get_Id_SelectedCharacter();
            pmm.Update_HeroCharactersView();
        }

        if(hCharacter)
        {
            pmm.selected_hCharacter_Name_Text.text = character.cName;
            pmm.selected_hCharacter_id = Get_Id_SelectedCharacter();
        }

        if(pCharacter)
        {
            pmm.selected_pCharacter_Name_Text.text = character.cName;
            pmm.selected_pCharacter_id = Get_Id_SelectedCharacter();
        }
	}

    private int Get_Id_SelectedCharacter()
    {
        Account acc = GameData.inst.account;
        int result = 0;

        if(hero)
        {
            for(int x = 0; x < acc.heroes.Count; x++)
                if(acc.heroes[x].character == character) result = x;
        }

        if(hCharacter)
        {
            for(int x = 0; x < character.cHero.battleCharacters.Count; x++)
                if(character.cHero.battleCharacters[x] == character) result = x;
        }

        if(pCharacter)
        {
            for(int x = 0; x < acc.сharacters.Count; x++)
                if(acc.сharacters[x] == character) result = x;
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
