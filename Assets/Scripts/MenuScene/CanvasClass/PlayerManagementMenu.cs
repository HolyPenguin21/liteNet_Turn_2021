using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagementMenu : UI_Menu_Canvas
{
    public MenuSceneMain mm;
    public Account account;

    public int selected_hero_id;
    public int selected_hCharacter_id;
    public int selected_pCharacter_id;

    public Text accGold_Text;
    public Text selected_hero_Name_Text;
    public Text selected_hCharacter_Name_Text;
    public Text selected_pCharacter_Name_Text;

    public Transform heroContent_tr;
    public Transform hCharactersContent_tr;
    public Transform pCharactersContent_tr;

    public GameObject heroCreation_Panel;
    public InputField creation_heroName;
    public int heroIdToCreate;
    public Button creation_heroSwordman_Button;
    public Button creation_heroSpearman_Button;
    public Button creation_heroKnight_Button;

    public GameObject characterBuy_Panel;
    public int characterIdToBuy;
    public Button buy_Swordman_Button;
    public Button buy_Spearman_Button;
    public Button buy_Knight_Button;
    public Text characterToBuy_info_Text;

    private CharactersData cd;

    public PlayerManagementMenu(GameObject gameObject, MenuSceneMain mm)
    {
        go = gameObject;

        this.mm = mm;

        accGold_Text = GameObject.Find("AccGold_Text").GetComponent<Text>();
        selected_hero_Name_Text = GameObject.Find("sHero_Name_Text").GetComponent<Text>();
        selected_hCharacter_Name_Text = GameObject.Find("s_hCharacter_Name_Text").GetComponent<Text>();
        selected_pCharacter_Name_Text = GameObject.Find("s_pCharacter_Name_Text").GetComponent<Text>();

        heroContent_tr = GameObject.Find("Hero_ScrollView").transform.Find("Viewport").transform.Find("Content").GetComponent<Transform>();
        hCharactersContent_tr = GameObject.Find("hCharacters_ScrollView").transform.Find("Viewport").transform.Find("Content").GetComponent<Transform>();
        pCharactersContent_tr = GameObject.Find("pCharacters_ScrollView").transform.Find("Viewport").transform.Find("Content").GetComponent<Transform>();

        heroCreation_Panel = go.transform.Find("HeroCreation_Panel").gameObject;
        creation_heroName = heroCreation_Panel.transform.Find("HeroName_InputField").GetComponent<InputField>();
        creation_heroSwordman_Button = heroCreation_Panel.transform.Find("HeroType_Swordman_Button").GetComponent<Button>();
        creation_heroSpearman_Button = heroCreation_Panel.transform.Find("HeroType_Spearman_Button").GetComponent<Button>();
        creation_heroKnight_Button = heroCreation_Panel.transform.Find("HeroType_Knight_Button").GetComponent<Button>();

        characterBuy_Panel = go.transform.Find("CharacterBuy_Panel").gameObject;
        buy_Swordman_Button = characterBuy_Panel.transform.Find("cType_Swordman_Button").GetComponent<Button>();
        buy_Spearman_Button = characterBuy_Panel.transform.Find("cType_Spearman_Button").GetComponent<Button>();
        buy_Knight_Button = characterBuy_Panel.transform.Find("cType_Knight_Button").GetComponent<Button>();
        characterToBuy_info_Text = characterBuy_Panel.transform.Find("CharacterToBuy_Info_Text").GetComponent<Text>();

        cd = new CharactersData();
    }


    #region Update UI
    public void Update_PlayerManagementMenu()
    {
        Update_PlayerInfo_UI();

        Update_HeroView();
        Update_HeroCharactersView();
        Update_PlayerCharactersView();
    }

    private void Update_PlayerInfo_UI()
    {
        accGold_Text.text = "Gold : " + account.acc_gold;
    }

    public void Update_HeroView()
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

            GameObject cButton_obj = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/Character_Button", typeof(GameObject)), heroContent_tr) as GameObject;
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
            GameObject hCharacterButton = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/Character_Button", typeof(GameObject)), hCharactersContent_tr) as GameObject;
            Menu_Character_Button menu_cButton = hCharacterButton.GetComponent<Menu_Character_Button>();
            
            menu_cButton.Init(this, c, Utility.UI_Char_Button.battleChar);
            if(x == 0) menu_cButton.TaskOnClick();
        }
    }

    public void Update_PlayerCharactersView()
    {
        // Clear
        foreach (Transform child in pCharactersContent_tr)
            if(child.gameObject.name != "BuyCharacter_Button")
                GameObject.Destroy(child.gameObject);

        if (account.сharacters.Count == 0) return;

        for (int x = 0; x < account.сharacters.Count; x++)
        {
            Character c = account.сharacters[x];
            GameObject hCharacterButton = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/Character_Button", typeof(GameObject)), pCharactersContent_tr) as GameObject;
            Menu_Character_Button menu_cButton = hCharacterButton.GetComponent<Menu_Character_Button>();

            menu_cButton.Init(this, c, Utility.UI_Char_Button.accChar);
            if(x == 0) menu_cButton.TaskOnClick();
        }
    }
    #endregion

    #region PMM buttons
    public void Button_PMM_SetForBattle_Hero()
    {
        account.battleHeroId = selected_hero_id;
    }

    public void Delete_Hero()
    {
        if(account.heroes.Count == 0) return;
        Hero h = account.heroes[selected_hero_id];

        List<Character> temp = new List<Character>();
        for(int x = 0; x < h.battleCharacters.Count; x++)
        {
            Character c = h.battleCharacters[x];
            temp.Add(c);
        }

        for(int x = 0; x < temp.Count; x++)
        {
            Character c = temp[x];
            account.сharacters.Add(c);
        }

        h.battleCharacters.Clear();
        account.heroes.Remove(h);

        if(account.heroes.Count == 0) selected_hero_Name_Text.text = "Hero info ...";

        if (account.battleHeroId == selected_hero_id) account.battleHeroId = 0;
        selected_hero_id = 0;
        selected_hCharacter_id = 0;

        Update_PlayerManagementMenu();
    }

    public void AddCharacterToHero()
    {
        if (account.heroes.Count == 0) return;
        Hero h = account.heroes[selected_hero_id];

        if (account.сharacters.Count == 0) return;
        Character c = account.сharacters[selected_pCharacter_id];

        if (h.battleCharacters.Count >= 5) return;

        account.сharacters.Remove(c);
        h.battleCharacters.Add(c);

        // UI changes
        selected_pCharacter_id = 0;
        if (account.сharacters.Count == 0) selected_pCharacter_Name_Text.text = "Character info ...";

        Update_HeroCharactersView();
        Update_PlayerCharactersView();
    }

    public void RemoveCharacterFromHero()
    {        
        if (account.heroes.Count == 0) return;
        Hero h = account.heroes[selected_hero_id];

        if (h.battleCharacters.Count == 0) return;
        Character c = h.battleCharacters[selected_hCharacter_id];

        h.battleCharacters.Remove(c);
        account.сharacters.Add(c);

        // UI changes
        selected_hCharacter_id = 0;
        if (h.battleCharacters.Count == 0) selected_hCharacter_Name_Text.text = "Hero character info ...";

        Update_HeroCharactersView();
        Update_PlayerCharactersView();
    }
    #endregion

    #region Hero creation panel
    public void Open_HeroCreation_Panel()
    {
        if (!heroCreation_Panel.activeInHierarchy)
            heroCreation_Panel.SetActive(true);
    }

    public void Select_Swordman_AsHero()
    {
        creation_heroSwordman_Button.interactable = false;
        creation_heroSpearman_Button.interactable = true;
        creation_heroKnight_Button.interactable = true;
        heroIdToCreate = 1;
    }

    public void Select_Spearman_AsHero()
    {
        creation_heroSwordman_Button.interactable = true;
        creation_heroSpearman_Button.interactable = false;
        creation_heroKnight_Button.interactable = true;
        heroIdToCreate = 2;
    }

    public void Select_Knight_AsHero()
    {
        creation_heroSwordman_Button.interactable = true;
        creation_heroSpearman_Button.interactable = true;
        creation_heroKnight_Button.interactable = false;
        heroIdToCreate = 3;
    }

    public void Create_Hero()
    {
        Character hCharacter = cd.Get_Character_ById(heroIdToCreate);
        string heroName = creation_heroName.text;
        if (heroName == "" || heroName == " " || heroName == "  " || heroName == "   ") return;
        Hero hero = new Hero(hCharacter, heroName);

        account.heroes.Add(hero);

        Update_HeroView();
        Update_HeroCharactersView();

        heroCreation_Panel.SetActive(false);

        LocalData ld = new LocalData();
        ld.Save_PlayerData(account);
    }

    public void Button_Back_HeroCreation()
    {
        heroCreation_Panel.SetActive(false);
    }
    #endregion

    #region Character purchase
    public void Open_CharacterBuy_Panel()
    {
        characterBuy_Panel.SetActive(true);
    }

    public void Select_Swordman_ToBuy()
    {
        buy_Swordman_Button.interactable = false;
        buy_Spearman_Button.interactable = true;
        buy_Knight_Button.interactable = true;
        characterIdToBuy = 1;
        Update_CharacterInfo_OnBuy(cd.Get_Character_ById(characterIdToBuy));
    }
    public void Select_Spearman_ToBuy()
    {
        buy_Swordman_Button.interactable = true;
        buy_Spearman_Button.interactable = false;
        buy_Knight_Button.interactable = true;
        characterIdToBuy = 2;
        Update_CharacterInfo_OnBuy(cd.Get_Character_ById(characterIdToBuy));
    }
    public void Select_Knight_ToBuy()
    {
        buy_Swordman_Button.interactable = true;
        buy_Spearman_Button.interactable = true;
        buy_Knight_Button.interactable = false;
        characterIdToBuy = 3;
        Update_CharacterInfo_OnBuy(cd.Get_Character_ById(characterIdToBuy));
    }

    private void Update_CharacterInfo_OnBuy(Character c)
    {
        characterToBuy_info_Text.text = c.name + "\n\n";
        characterToBuy_info_Text.text += c.cost + "\n\n";
        characterToBuy_info_Text.text += "Attack : \n";
        characterToBuy_info_Text.text += "Defence : \n";
        characterToBuy_info_Text.text += "Skills : \n";
    }

    public void Buy_Character()
    {
        Debug.Log("Character to buy id : " + characterIdToBuy);
        Character c = cd.Get_Character_ById(characterIdToBuy);

        if(account.acc_gold < c.cost) return;
        
        account.acc_gold -= c.cost;
        account.сharacters.Add(c);

        Button_Back_CharacterBuy();
        Update_PlayerInfo_UI();
        Update_PlayerCharactersView();

        LocalData ld = new LocalData();
        ld.Save_PlayerData(account);
    }

    public void Button_Back_CharacterBuy()
    {
        characterBuy_Panel.SetActive(false);
    }
    #endregion
}
