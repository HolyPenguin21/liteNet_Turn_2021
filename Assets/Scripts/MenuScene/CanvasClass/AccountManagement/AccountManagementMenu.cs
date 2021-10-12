using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountManagementMenu : UI_Menu_Canvas
{
    public MenuSceneMain menuSceneMain;

    private Button overview_Button;
    public GameObject overviewPanel_go;
    private AccountOverview accountOverview;

    private Button characters_Button;
    public GameObject charactersPanel_go;
    private CharactersPanel charactersPanel;

    private Button items_Button;
    public GameObject itemsPanel_go;
    private ItemsPanel itemsPanel;

    public GameObject buyHeroPanel_go;
    private BuyHero buyHeroPanel;

    public GameObject buyCharacterPanel_go;
    public BuyCharacter buyCharacterPanel;

    private Text accGold_Text;

    private CharactersData cd;

    public AccountManagementMenu(MenuSceneMain menuSceneMain)
    {
        this.menuSceneMain = menuSceneMain;
        this.go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/AcountManagement_Canvas", typeof(GameObject))) as GameObject;

        this.accountOverview = new AccountOverview(this);
        this.charactersPanel = new CharactersPanel(this);
        this.itemsPanel = new ItemsPanel(this);

        this.buyHeroPanel_go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/BuyHero/BuyHero_Panel", typeof(GameObject)), go.transform) as GameObject;
        this.buyHeroPanel = new BuyHero(this);
        this.buyHeroPanel.Hide();

        this.buyCharacterPanel_go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/BuyCharacterPanel/BuyCharacter_Panel", typeof(GameObject)), go.transform) as GameObject;
        this.buyCharacterPanel = new BuyCharacter(this);
        this.buyCharacterPanel.Hide();

        this.overview_Button = go.transform.Find("Overview_Button").GetComponent<Button>();
        this.overview_Button.onClick.AddListener(Overview_Button);

        this.characters_Button = go.transform.Find("Characters_Button").GetComponent<Button>();
        this.characters_Button.onClick.AddListener(Characters_Button);

        this.items_Button = go.transform.Find("Items_Button").GetComponent<Button>();
        this.items_Button.onClick.AddListener(Items_Button);

        accGold_Text = go.transform.Find("Gold_Panel").transform.Find("Text").GetComponent<Text>();

        Hide();

        cd = new CharactersData();
    }

    public void Overview_Button()
    {
        overview_Button.interactable = false;
        characters_Button.interactable = true;
        items_Button.interactable = true;

        Update_AccountManagementMenu();

        charactersPanel.Hide();
        buyCharacterPanel.Hide();
        buyHeroPanel.Hide();
        itemsPanel.Hide();
        accountOverview.Show();
    }

    private void Characters_Button()
    {
        overview_Button.interactable = true;
        characters_Button.interactable = false;
        items_Button.interactable = true;

        Update_AccountManagementMenu();
        charactersPanel.Update_UI();

        accountOverview.Hide();
        itemsPanel.Hide();
        charactersPanel.Show();
    }

    private void Items_Button()
    {
        overview_Button.interactable = true;
        characters_Button.interactable = true;
        items_Button.interactable = false;

        Update_AccountManagementMenu();

        accountOverview.Hide();
        charactersPanel.Hide();
        buyCharacterPanel.Hide();
        buyHeroPanel.Hide();
        itemsPanel.Show();
    }

    public void Update_AccountManagementMenu()
    {
        accGold_Text.text = "Gold : " + GameData.inst.account.acc_gold;
    }

    public void Create_Hero(int heroIdToCreate, string heroName_Input)
    {
        Character hCharacter = cd.Get_Character_ById(heroIdToCreate);
        Hero hero = new Hero(hCharacter, heroName_Input);

        GameData.inst.account.heroes.Add(hero);

        charactersPanel.Update_HeroView();
        charactersPanel.Update_HeroCharactersView();

        buyHeroPanel.Hide();

        LocalData ld = new LocalData();
        ld.Save_PlayerData(GameData.inst.account);
    }

    public void Buy_Character(int characterIdToBuy)
    {
        Character c = cd.Get_Character_ById(characterIdToBuy);
        
        GameData.inst.account.acc_gold -= c.acc_cost;
        GameData.inst.account.сharacters.Add(c);

        buyCharacterPanel.Hide();
        charactersPanel.Update_PlayerCharactersView();
        Update_AccountManagementMenu();

        LocalData ld = new LocalData();
        ld.Save_PlayerData(GameData.inst.account);
    }
}
