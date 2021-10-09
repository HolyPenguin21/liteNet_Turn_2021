using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyCharacter
{
    private AccountManagementMenu accountManagementMenu;

    private Transform charactersContent_tr;
    private List<BuyCharacter_Button> charButtons = new List<BuyCharacter_Button>();
    private Text characterToBuy_info_Text;
    public int characterIdToBuy;

    private Button buy_Button;
    private Button back_Button;

    private CharactersData cd;

    public BuyCharacter(AccountManagementMenu accountManagementMenu)
    {
        this.accountManagementMenu = accountManagementMenu;

        this.charactersContent_tr = accountManagementMenu.buyCharacterPanel_go.transform.Find("Characters_View").transform.Find("Viewport").transform.Find("Content").GetComponent<Transform>();

        this.characterToBuy_info_Text = accountManagementMenu.buyCharacterPanel_go.transform.Find("CharacterToBuy_Info_Text").GetComponent<Text>();

        this.back_Button = accountManagementMenu.buyCharacterPanel_go.transform.Find("Buy_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(Buy_Button);

        this.back_Button = accountManagementMenu.buyCharacterPanel_go.transform.Find("Back_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(Back_Button);

        cd = new CharactersData();
    }

    public void Show()
    {
        // Clear
        charButtons.Clear();
        foreach (Transform child in charactersContent_tr)
            GameObject.Destroy(child.gameObject);
        
        for(int x = 1; x < cd.currentCharactersCount; x++)
        {
            Character character = cd.Get_Character_ById(x);
            GameObject button_go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/BuyCharacterPanel/BuyCharacter_Button", typeof(GameObject)), charactersContent_tr) as GameObject;
            BuyCharacter_Button buyCharacter_Button = button_go.GetComponent<BuyCharacter_Button>();
            buyCharacter_Button.Init(GameData.inst.account, this, character);
            
            charButtons.Add(buyCharacter_Button);
        }
    }

    public void Update_CharButtons()
    {
        for(int x = 0; x < charButtons.Count; x++)
        {
            BuyCharacter_Button charButton = charButtons[x];

            if(charButton.Check_AvailableOnAccount())
                charButton.button.interactable = true;
            else
                charButton.button.interactable = false;
        }
    }

    public void Buy_Button()
    {
        accountManagementMenu.Buy_Character(characterIdToBuy);
    }

    public void Back_Button()
    {
        accountManagementMenu.buyCharacterPanel_go.SetActive(false);
    }

    public void Update_CharacterInfo_OnBuy(Character character)
    {
        characterToBuy_info_Text.text = cd.Get_Menu_Character_Tooltip(character);
    }
}
