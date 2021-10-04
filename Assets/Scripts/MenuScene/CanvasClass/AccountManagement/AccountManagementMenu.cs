using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountManagementMenu : UI_Menu_Canvas
{
    public MenuSceneMain menuSceneMain;
    public Account account;

    private Button overview_Button;
    public GameObject overviewPanel_go;
    private AccountOverview accountOverview;

    private Button characters_Button;
    public GameObject charactersPanel_go;
    private CharactersPanel charactersPanel;

    private Button items_Button;
    public GameObject itemsPanel_go;
    private ItemsPanel itemsPanel;

    private Text accGold_Text;

    // public GameObject heroCreation_Panel;
    // public InputField creation_heroName;
    // public int heroIdToCreate;
    // public Button creation_heroSwordman_Button;
    // public Button creation_heroSpearman_Button;
    // public Button creation_heroKnight_Button;

    // public GameObject characterBuy_Panel;
    // public int characterIdToBuy;
    // public Button buy_Swordman_Button;
    // public Button buy_Spearman_Button;
    // public Button buy_Knight_Button;
    // public Text characterToBuy_info_Text;

    private CharactersData cd;

    public AccountManagementMenu(MenuSceneMain menuSceneMain)
    {
        this.menuSceneMain = menuSceneMain;
        this.go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/AcountManagement_Canvas", typeof(GameObject))) as GameObject;

        this.accountOverview = new AccountOverview(this);
        this.charactersPanel = new CharactersPanel(this);
        this.itemsPanel = new ItemsPanel(this);

        this.overview_Button = go.transform.Find("Overview_Button").GetComponent<Button>();
        this.overview_Button.onClick.AddListener(Overview_Button);

        this.characters_Button = go.transform.Find("Characters_Button").GetComponent<Button>();
        this.characters_Button.onClick.AddListener(Characters_Button);

        this.items_Button = go.transform.Find("Items_Button").GetComponent<Button>();
        this.items_Button.onClick.AddListener(Items_Button);

        accGold_Text = go.transform.Find("Gold_Panel").transform.Find("Text").GetComponent<Text>();

        Hide();

        // heroCreation_Panel = go.transform.Find("HeroCreation_Panel").gameObject;
        // creation_heroName = heroCreation_Panel.transform.Find("HeroName_InputField").GetComponent<InputField>();
        // creation_heroSwordman_Button = heroCreation_Panel.transform.Find("HeroType_Swordman_Button").GetComponent<Button>();
        // creation_heroSpearman_Button = heroCreation_Panel.transform.Find("HeroType_Spearman_Button").GetComponent<Button>();
        // creation_heroKnight_Button = heroCreation_Panel.transform.Find("HeroType_Knight_Button").GetComponent<Button>();

        // characterBuy_Panel = go.transform.Find("CharacterBuy_Panel").gameObject;
        // buy_Swordman_Button = characterBuy_Panel.transform.Find("cType_Swordman_Button").GetComponent<Button>();
        // buy_Spearman_Button = characterBuy_Panel.transform.Find("cType_Spearman_Button").GetComponent<Button>();
        // buy_Knight_Button = characterBuy_Panel.transform.Find("cType_Knight_Button").GetComponent<Button>();
        // characterToBuy_info_Text = characterBuy_Panel.transform.Find("CharacterToBuy_Info_Text").GetComponent<Text>();

        // cd = new CharactersData();
    }

    public void Overview_Button()
    {
        overview_Button.interactable = false;
        characters_Button.interactable = true;
        items_Button.interactable = true;

        Update_AccountManagementMenu();

        accountOverview.Show();
        charactersPanel.Hide();
        itemsPanel.Hide();
    }

    private void Characters_Button()
    {
        overview_Button.interactable = true;
        characters_Button.interactable = false;
        items_Button.interactable = true;

        Update_AccountManagementMenu();
        charactersPanel.Update_UI();

        charactersPanel.Show();
        accountOverview.Hide();
        itemsPanel.Hide();
    }

    private void Items_Button()
    {
        overview_Button.interactable = true;
        characters_Button.interactable = true;
        items_Button.interactable = false;

        Update_AccountManagementMenu();

        accountOverview.Hide();
        charactersPanel.Hide();
        itemsPanel.Show();
    }

    public void Update_AccountManagementMenu()
    {
        if(this.account == null) 
        {
            this.account = GameData.inst.account;
            this.charactersPanel.account = this.account;
        }
        accGold_Text.text = "Gold : " + account.acc_gold;
    }

    #region Hero creation panel
    // public void Open_HeroCreation_Panel()
    // {
    //     if (!heroCreation_Panel.activeInHierarchy)
    //         heroCreation_Panel.SetActive(true);
    // }

    // public void Select_Swordman_AsHero()
    // {
    //     creation_heroSwordman_Button.interactable = false;
    //     creation_heroSpearman_Button.interactable = true;
    //     creation_heroKnight_Button.interactable = true;
    //     heroIdToCreate = 1;
    // }

    // public void Select_Spearman_AsHero()
    // {
    //     creation_heroSwordman_Button.interactable = true;
    //     creation_heroSpearman_Button.interactable = false;
    //     creation_heroKnight_Button.interactable = true;
    //     heroIdToCreate = 2;
    // }

    // public void Select_Knight_AsHero()
    // {
    //     creation_heroSwordman_Button.interactable = true;
    //     creation_heroSpearman_Button.interactable = true;
    //     creation_heroKnight_Button.interactable = false;
    //     heroIdToCreate = 3;
    // }

    // public void Create_Hero()
    // {
    //     Character hCharacter = cd.Get_Character_ById(heroIdToCreate);
    //     string heroName = creation_heroName.text;
    //     if (heroName == "" || heroName == " " || heroName == "  " || heroName == "   ") return;
    //     Hero hero = new Hero(hCharacter, heroName);

    //     account.heroes.Add(hero);

    //     Update_HeroView();
    //     Update_HeroCharactersView();

    //     heroCreation_Panel.SetActive(false);

    //     LocalData ld = new LocalData();
    //     ld.Save_PlayerData(account);
    // }

    // public void Button_Back_HeroCreation()
    // {
    //     heroCreation_Panel.SetActive(false);
    // }
    #endregion

    #region Character purchase
    // public void Open_CharacterBuy_Panel()
    // {
    //     characterBuy_Panel.SetActive(true);
    // }

    // public void Select_Swordman_ToBuy()
    // {
    //     buy_Swordman_Button.interactable = false;
    //     buy_Spearman_Button.interactable = true;
    //     buy_Knight_Button.interactable = true;
    //     characterIdToBuy = 1;
    //     Update_CharacterInfo_OnBuy(cd.Get_Character_ById(characterIdToBuy));
    // }
    // public void Select_Spearman_ToBuy()
    // {
    //     buy_Swordman_Button.interactable = true;
    //     buy_Spearman_Button.interactable = false;
    //     buy_Knight_Button.interactable = true;
    //     characterIdToBuy = 2;
    //     Update_CharacterInfo_OnBuy(cd.Get_Character_ById(characterIdToBuy));
    // }
    // public void Select_Knight_ToBuy()
    // {
    //     buy_Swordman_Button.interactable = true;
    //     buy_Spearman_Button.interactable = true;
    //     buy_Knight_Button.interactable = false;
    //     characterIdToBuy = 3;
    //     Update_CharacterInfo_OnBuy(cd.Get_Character_ById(characterIdToBuy));
    // }

    // private void Update_CharacterInfo_OnBuy(Character c)
    // {
    //     characterToBuy_info_Text.text = c.name + "\n\n";
    //     characterToBuy_info_Text.text += c.acc_cost + "\n\n";
    //     characterToBuy_info_Text.text += "Attack : \n";
    //     characterToBuy_info_Text.text += "Defence : \n";
    //     characterToBuy_info_Text.text += "Skills : \n";
    // }

    // public void Buy_Character()
    // {
    //     Debug.Log("Character to buy id : " + characterIdToBuy);
    //     Character c = cd.Get_Character_ById(characterIdToBuy);

    //     if(account.acc_gold < c.acc_cost) return;
        
    //     account.acc_gold -= c.acc_cost;
    //     account.сharacters.Add(c);

    //     Button_Back_CharacterBuy();
    //     Update_PlayerInfo_UI();
    //     Update_PlayerCharactersView();

    //     LocalData ld = new LocalData();
    //     ld.Save_PlayerData(account);
    // }

    // public void Button_Back_CharacterBuy()
    // {
    //     characterBuy_Panel.SetActive(false);
    // }
    #endregion
}
