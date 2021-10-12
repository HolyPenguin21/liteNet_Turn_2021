using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyHero
{
    private AccountManagementMenu accountManagementMenu;

    private InputField heroName_Input;
    private Text heroInfo_Text;
    private Button heroSwordman_Button;
    private Button heroSpearman_Button;
    private int heroIdToCreate;

    private Button buy_Button;
    private Button back_Button;

    public BuyHero(AccountManagementMenu accountManagementMenu)
    {
        this.accountManagementMenu = accountManagementMenu;

        this.heroName_Input = accountManagementMenu.buyHeroPanel_go.transform.Find("HeroName_InputField").GetComponent<InputField>();
        this.heroInfo_Text = accountManagementMenu.buyHeroPanel_go.transform.Find("HeroInfo_Text").GetComponent<Text>();

        this.heroSwordman_Button = accountManagementMenu.buyHeroPanel_go.transform.Find("HeroType_Swordman_Button").GetComponent<Button>();
        this.heroSwordman_Button.onClick.AddListener(Select_Swordman_AsHero);
        
        this.heroSpearman_Button = accountManagementMenu.buyHeroPanel_go.transform.Find("HeroType_Spearman_Button").GetComponent<Button>();
        this.heroSpearman_Button.onClick.AddListener(Select_Spearman_AsHero);

        this.buy_Button = accountManagementMenu.buyHeroPanel_go.transform.Find("Buy_Button").GetComponent<Button>();
        this.buy_Button.onClick.AddListener(Buy_Button);

        this.back_Button = accountManagementMenu.buyHeroPanel_go.transform.Find("Back_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(Hide);
    }

    public void Select_Swordman_AsHero()
    {
        heroSwordman_Button.interactable = false;
        heroSpearman_Button.interactable = true;
        heroIdToCreate = 1;
    }

    public void Select_Spearman_AsHero()
    {
        heroSwordman_Button.interactable = true;
        heroSpearman_Button.interactable = false;
        heroIdToCreate = 2;
    }

    public void Buy_Button()
    {
        if(heroName_Input.text == "") 
            heroName_Input.text = "SomeHeroName_" + Random.Range(1, 123);

        accountManagementMenu.Create_Hero(heroIdToCreate, heroName_Input.text);
    }

    public void Hide()
    {
        accountManagementMenu.buyHeroPanel_go.SetActive(false);
    }
}
