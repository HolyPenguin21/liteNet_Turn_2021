using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyCharacter_Button : MonoBehaviour
{
    private Account account;
    private BuyCharacter buyCharacter;
    private Character character;

    public Button button;
    private Image charImage;
    private Text charText;

    public void Init(Account account, BuyCharacter buyCharacter, Character character)
    {
        this.button = GetComponent<Button>();
        this.button.onClick.AddListener(TaskOnClick);

        this.account = account;
        this.buyCharacter = buyCharacter;
        this.character = character;

        this.charImage = transform.Find("Image").GetComponent<Image>();
        this.charImage.sprite = character.image;

        this.charText = transform.Find("Text").GetComponent<Text>();
        this.charText.text = character.name + " - " + character.faction + " - " + character.acc_cost;

        if(Check_AvailableOnAccount())
            button.interactable = true;
        else
            button.interactable = false;
    }

    public void TaskOnClick()
    {
        buyCharacter.Update_CharButtons();
        button.interactable = false;
        buyCharacter.characterIdToBuy = character.id;
        buyCharacter.Update_CharacterInfo_OnBuy(character);
	}

    public bool Check_AvailableOnAccount()
    {
        switch (character.faction)
        {
            case CharVars.faction.General:
                for(int x = 0; x < account.items.Count; x++)
                    if(account.items[x].id == 2) return true;
            break;
            case CharVars.faction.Castle:
                for(int x = 0; x < account.items.Count; x++)
                    if(account.items[x].id == 3) return true;
            break;

            case CharVars.faction.Forest:
                for(int x = 0; x < account.items.Count; x++)
                    if(account.items[x].id == 4) return true;

            break;
            case CharVars.faction.Dark:
                for(int x = 0; x < account.items.Count; x++)
                    if(account.items[x].id == 5) return true;
            break;

            default:
                return false;
        }

        return false;
    }
}
