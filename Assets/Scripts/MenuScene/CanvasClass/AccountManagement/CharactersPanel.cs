using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactersPanel
{
    private AccountManagementMenu accountManagementMenu;
    public Account account;

    public Text selected_hero_Text;
    public Text selected_hCharacter_Text;
    public Text selected_pCharacter_Text;

    private Transform heroContent_tr;
    private Transform hCharactersContent_tr;
    private Transform pCharactersContent_tr;

    private Button back_Button;

    public int selected_hero_id;
    public int selected_hCharacter_id;
    public int selected_pCharacter_id;

    public CharactersPanel(AccountManagementMenu accountManagementMenu)
    {
        this.accountManagementMenu = accountManagementMenu;
        account = accountManagementMenu.account;
        accountManagementMenu.charactersPanel_go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/Characters_Panel", typeof(GameObject)), accountManagementMenu.go.transform) as GameObject;

        this.selected_hero_Text = accountManagementMenu.charactersPanel_go.transform.Find("sHero_Text").GetComponent<Text>();
        this.selected_hCharacter_Text = accountManagementMenu.charactersPanel_go.transform.Find("s_hCharacter_Text").GetComponent<Text>();
        this.selected_pCharacter_Text = accountManagementMenu.charactersPanel_go.transform.Find("s_pCharacter_Text").GetComponent<Text>();

        heroContent_tr = accountManagementMenu.charactersPanel_go.transform.Find("Hero_ScrollView").transform.Find("Viewport").transform.Find("Content").GetComponent<Transform>();
        hCharactersContent_tr = accountManagementMenu.charactersPanel_go.transform.Find("hCharacters_ScrollView").transform.Find("Viewport").transform.Find("Content").GetComponent<Transform>();
        pCharactersContent_tr = accountManagementMenu.charactersPanel_go.transform.Find("pCharacters_ScrollView").transform.Find("Viewport").transform.Find("Content").GetComponent<Transform>();

        this.back_Button = accountManagementMenu.charactersPanel_go.transform.Find("Back_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(Back);

        Hide();
    }

    public void Update_UI()
    {
        Update_HeroView();
        Update_HeroCharactersView();
        Update_PlayerCharactersView();
    }

    private void Update_HeroView()
    {
        // Clear
        foreach (Transform child in heroContent_tr)
            if(child.gameObject.name != "CreateHero_Button")
                GameObject.Destroy(child.gameObject);

        if (account.heroes.Count == 0) return;

        // Rebuild
        for (int x = 0; x < account.heroes.Count; x++)
        {
            Hero h = account.heroes[x];

            GameObject cButton_obj = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/Character_Button", typeof(GameObject)), heroContent_tr) as GameObject;
            
            Menu_Character_Button menu_cButton = cButton_obj.GetComponent<Menu_Character_Button>();
            menu_cButton.hero = h;
            menu_cButton.Init(this, h.character, Utility.UI_Char_Button.hero);
            if(account.battleHeroId == x) menu_cButton.TaskOnClick();
        }
    }

    public void Update_HeroCharactersView()
    {
        // Clear
        foreach (Transform child in hCharactersContent_tr)
            GameObject.Destroy(child.gameObject);

        if (account.heroes.Count == 0) return;

        Hero h = account.heroes[selected_hero_id];
        if (h.battleCharacters.Count == 0) return;

        // Rebuild
        for (int x = 0; x < h.battleCharacters.Count; x++)
        {
            Character c = h.battleCharacters[x];
            GameObject hCharacterButton = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/Character_Button", typeof(GameObject)), hCharactersContent_tr) as GameObject;
            Menu_Character_Button menu_cButton = hCharacterButton.GetComponent<Menu_Character_Button>();
            
            menu_cButton.Init(this, c, Utility.UI_Char_Button.battleChar);
            if(x == 0) menu_cButton.TaskOnClick();
        }
    }

    private void Update_PlayerCharactersView()
    {
        // Clear
        foreach (Transform child in pCharactersContent_tr)
            if(child.gameObject.name != "BuyCharacter_Button")
                GameObject.Destroy(child.gameObject);

        if (account.сharacters.Count == 0) return;

        for (int x = 0; x < account.сharacters.Count; x++)
        {
            Character c = account.сharacters[x];
            GameObject hCharacterButton = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/Character_Button", typeof(GameObject)), pCharactersContent_tr) as GameObject;
            Menu_Character_Button menu_cButton = hCharacterButton.GetComponent<Menu_Character_Button>();

            menu_cButton.Init(this, c, Utility.UI_Char_Button.accChar);
            if(x == 0) menu_cButton.TaskOnClick();
        }
    }

    public void Delete_Hero()
    {
        // if(account.heroes.Count == 0) return;
        // Hero h = account.heroes[selected_hero_id];

        // List<Character> temp = new List<Character>();
        // for(int x = 0; x < h.battleCharacters.Count; x++)
        // {
        //     Character c = h.battleCharacters[x];
        //     temp.Add(c);
        // }

        // for(int x = 0; x < temp.Count; x++)
        // {
        //     Character c = temp[x];
        //     account.сharacters.Add(c);
        // }

        // h.battleCharacters.Clear();
        // account.heroes.Remove(h);

        // if(account.heroes.Count == 0) selected_hero_Name_Text.text = "Hero info ...";

        // if (account.battleHeroId == selected_hero_id) account.battleHeroId = 0;
        // selected_hero_id = 0;
        // selected_hCharacter_id = 0;

        // Update_PlayerManagementMenu();
    }

    public void AddCharacterToHero()
    {
        // if (account.heroes.Count == 0) return;
        // Hero h = account.heroes[selected_hero_id];

        // if (account.сharacters.Count == 0) return;
        // Character c = account.сharacters[selected_pCharacter_id];

        // if (h.battleCharacters.Count >= 5) return;

        // account.сharacters.Remove(c);
        // h.battleCharacters.Add(c);

        // // UI changes
        // selected_pCharacter_id = 0;
        // if (account.сharacters.Count == 0) selected_pCharacter_Name_Text.text = "Character info ...";

        // Update_HeroCharactersView();
        // Update_PlayerCharactersView();
    }

    public void RemoveCharacterFromHero()
    {        
        // if (account.heroes.Count == 0) return;
        // Hero h = account.heroes[selected_hero_id];

        // if (h.battleCharacters.Count == 0) return;
        // Character c = h.battleCharacters[selected_hCharacter_id];

        // h.battleCharacters.Remove(c);
        // account.сharacters.Add(c);

        // // UI changes
        // selected_hCharacter_id = 0;
        // if (h.battleCharacters.Count == 0) selected_hCharacter_Name_Text.text = "Hero character info ...";

        // Update_HeroCharactersView();
        // Update_PlayerCharactersView();
    }

    public void SetForBattle_Hero()
    {
        account.battleHeroId = selected_hero_id;
    }

    public void Show()
    {
        accountManagementMenu.charactersPanel_go.SetActive(true);
    }

    private void Back()
    {
        accountManagementMenu.menuSceneMain.Button_AccountManagement_Back();
    }

    public void Hide()
    {
        accountManagementMenu.charactersPanel_go.SetActive(false);
    }
}
