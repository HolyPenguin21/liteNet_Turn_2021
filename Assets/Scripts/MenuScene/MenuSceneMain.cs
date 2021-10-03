using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneMain : MonoBehaviour
{
    public MainMenu mainMenu;
    public LocalLogin localLogin_Menu;
    public AccountOverview accountOverview_Menu;
    public P2PMenu p2pMenu;
    public PlayerManagementMenu playerManagementMenu;
    public P2PLobbyMenu p2PLobbyMenu;
    public OfflineMenu offlineMenu;

    private void Awake()
    {
        localLogin_Menu = new LocalLogin(this);
        accountOverview_Menu = new AccountOverview(this);
        mainMenu = new MainMenu(this);
        p2pMenu = new P2PMenu(this);

        playerManagementMenu = new PlayerManagementMenu(GameObject.Find("PlayerManagement_Canvas"), this);
        p2PLobbyMenu = new P2PLobbyMenu(GameObject.Find("P2P_Lobby_Canvas"));
        offlineMenu = new OfflineMenu(GameObject.Find("Offine_Canvas"));

        HideBeforeStart();
    }

    private void HideBeforeStart()
    {
        accountOverview_Menu.Hide();
        mainMenu.Hide();
        p2pMenu.Hide();

        playerManagementMenu.Hide();
        playerManagementMenu.heroCreation_Panel.SetActive(false);
        playerManagementMenu.characterBuy_Panel.SetActive(false);
        p2PLobbyMenu.Hide();
        offlineMenu.Hide();
    }

    private void Start()
    {
        if(GameData.inst == null) return;
        if(GameData.inst.account == null) return;

        localLogin_Menu.Hide();

        SingIn();
    }

    #region LocalLogin menu
    public void SingIn()
    {
        accountOverview_Menu.Show();
        accountOverview_Menu.Update_PlayerInfo();

        mainMenu.Show();
        p2pMenu.CheckPlayer();

        playerManagementMenu.account = GameData.inst.account;
    }
    #endregion

    #region AccountOverview menu
    public void AccountOverview_ManageAccount()
    {
        accountOverview_Menu.Hide();
        mainMenu.Hide();
        p2pMenu.Hide();
        offlineMenu.Hide();

        playerManagementMenu.Show();
        playerManagementMenu.Update_PlayerManagementMenu();
    }

    public void AccountOverview_Back()
    {
        accountOverview_Menu.Hide();

        p2pMenu.Hide();
        mainMenu.Hide();

        localLogin_Menu.Show();
    }
    #endregion

    #region Player Management Menu
    public void Button_PMM_HeroCreation()
    {
        playerManagementMenu.Open_HeroCreation_Panel();
    }
    public void Button_PMM_HeroCreation_Back()
    {
        playerManagementMenu.Button_Back_HeroCreation();
    }

    public void Button_PMM_BuyCharacter()
    {
        playerManagementMenu.Open_CharacterBuy_Panel();
    }
    public void Button_PMM_BuyCharacter_Back()
    {
        playerManagementMenu.Button_Back_CharacterBuy();
    }

    public void Button_PMM_SetForBattle_Hero()
    {
        playerManagementMenu.Button_PMM_SetForBattle_Hero();
    }

    public void Button_PMM_Delete_Hero()
    {
        playerManagementMenu.Delete_Hero();
    }

    public void On_Hero_Select()
    {
        playerManagementMenu.Update_HeroCharactersView();
    }

    public void Button_PMM_AddCharacterToHero()
    {
        playerManagementMenu.AddCharacterToHero();
    }

    public void Button_PMM_RemoveCharacterFromHero()
    {
        playerManagementMenu.RemoveCharacterFromHero();
    }

    // Hero Creation
    public void Button_HeroCreation_CreateHero()
    {
        playerManagementMenu.Create_Hero();
    }
    public void Button_HeroType_Swordman()
    {
        playerManagementMenu.Select_Swordman_AsHero();
    }
    public void Button_HeroType_Spearman()
    {
        playerManagementMenu.Select_Spearman_AsHero();
    }
    public void Button_HeroType_Knight()
    {
        playerManagementMenu.Select_Knight_AsHero();
    }

    // Character Buy
    public void Button_CharacterBuy_Buy()
    {
        playerManagementMenu.Buy_Character();
    }
    public void Button_Buy_Swordman()
    {
        playerManagementMenu.Select_Swordman_ToBuy();
    }
    public void Button_Buy_Spearman()
    {
        playerManagementMenu.Select_Spearman_ToBuy();
    }
    public void Button_Buy_Knight()
    {
        playerManagementMenu.Select_Knight_ToBuy();
    }

    public void Button_PMM_Back()
    {
        LocalData ld = new LocalData();
        ld.Save_PlayerData(GameData.inst.account);

        playerManagementMenu.Hide();
        accountOverview_Menu.Show();
        mainMenu.Show();

        accountOverview_Menu.Update_PlayerInfo();
    }
    #endregion

    #region P2P Menu
    public void Button_P2P()
    {
        mainMenu.Hide();
        p2pMenu.Show();
        p2pMenu.CheckPlayer();

        GameData.inst.gameType = Utility.GameType.pvp;
    }

    public void Host()
    {
        p2pMenu.Hide();
        accountOverview_Menu.Hide();
        GameData.inst.CreateHost();
        p2PLobbyMenu.Show();

        p2PLobbyMenu.startGame_Button.interactable = true;
    }

    public void Button_Back_P2P()
    {
        GameData.inst.Close_ServerClient();

        p2pMenu.Hide();
        mainMenu.Show();
    }
    #endregion

    #region P2P Lobby
    public void UpdateConnectedPlayersList()
    {
        p2PLobbyMenu.UpdateConnectedPlayersList();
    }

    public void Client_OpenLobby_OnConnect()
    {
        p2pMenu.Hide();
        accountOverview_Menu.Hide();
        p2PLobbyMenu.Show();
    }

    public void Add_PlayerPanel(Account player)
    {
        int pos = 0;
        if(GameData.inst.server != null) pos = Utility.Get_Server_PlayerID_ByName(player.name);
        if(GameData.inst.client != null) pos = Utility.Get_Client_PlayerID_ByName(player.name);
        p2PLobbyMenu.Add_PlayerPanel(pos, player);
    }

    public void Remove_PlayerPanel(Account player)
    {
        p2PLobbyMenu.Remove_PlayerPanel(player);
    }

    public void Button_StartGame()
    {
        GameData.inst.gameMain.Order_StartGame(3);
    }

    public void Button_P2P_Lobby_Back()
    {
        GameData.inst.Close_ServerClient();
        p2PLobbyMenu.Remove_AllPlayerPanels();
        p2PLobbyMenu.Hide();
        mainMenu.Show();
        accountOverview_Menu.Show();

        GameData.inst.account.isServer = false;
    }
    #endregion

    #region Offline Menu
    public void Button_Offline()
    {
        mainMenu.Hide();
        offlineMenu.Show();

        GameData.inst.gameType = Utility.GameType.solo;
    }

    public void Button_Events()
    {
        // Utility.Load_Scene(1);
    }

    public void Button_Offline_Back()
    {
        offlineMenu.Hide();
        mainMenu.Show();
    }
    #endregion
}
