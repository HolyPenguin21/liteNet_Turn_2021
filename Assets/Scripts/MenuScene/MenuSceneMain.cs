using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneMain : MonoBehaviour
{
    public MainMenu main_Menu;
    public LocalLogin localLogin_Menu;
    public AccountFastOverview accountFastOverview_Menu;
    public P2PMenu p2p_Menu;
    public AccountManagementMenu accountManagement_Menu;
    public P2PLobbyMenu p2PLobbyMenu;
    public OfflineMenu offlineMenu;

    private void Awake()
    {
        localLogin_Menu = new LocalLogin(this);
        accountFastOverview_Menu = new AccountFastOverview(this);
        main_Menu = new MainMenu(this);
        p2p_Menu = new P2PMenu(this);
        accountManagement_Menu = new AccountManagementMenu(this);
        offlineMenu = new OfflineMenu(this);
        p2PLobbyMenu = new P2PLobbyMenu(this);

        HideBeforeStart();
    }

    private void HideBeforeStart()
    {
        accountFastOverview_Menu.Hide();
        main_Menu.Hide();
        p2p_Menu.Hide();
        accountManagement_Menu.Hide();
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
        accountFastOverview_Menu.Show();
        accountFastOverview_Menu.Update_PlayerInfo();

        main_Menu.Show();
        p2p_Menu.CheckPlayer();
    }
    #endregion

    #region AccountOverview menu
    public void AccountOverview_ManageAccount()
    {
        accountFastOverview_Menu.Hide();
        main_Menu.Hide();
        p2p_Menu.Hide();
        offlineMenu.Hide();

        accountManagement_Menu.Update_AccountManagementMenu();
        accountManagement_Menu.Show();
        accountManagement_Menu.Overview_Button();
    }

    public void AccountOverview_Back()
    {
        accountFastOverview_Menu.Hide();

        main_Menu.Hide();
        p2p_Menu.Hide();
        offlineMenu.Hide();

        localLogin_Menu.Show();
    }
    #endregion

    #region Player Management Menu
    public void Button_AccountManagement_Back()
    {
        LocalData ld = new LocalData();
        ld.Save_PlayerData(GameData.inst.account);

        accountManagement_Menu.Hide();
        accountFastOverview_Menu.Show();
        main_Menu.Show();

        accountFastOverview_Menu.Update_PlayerInfo();
    }
    #endregion

    #region P2P Menu
    public void Button_P2P()
    {
        main_Menu.Hide();
        p2p_Menu.Show();
        p2p_Menu.CheckPlayer();

        GameData.inst.gameType = Utility.GameType.pvp;
    }

    public void Host()
    {
        p2p_Menu.Hide();
        accountFastOverview_Menu.Hide();
        GameData.inst.CreateHost();
        
        p2PLobbyMenu.startGame_Button.interactable = true;
        p2PLobbyMenu.Show();
    }

    public void Button_Back_P2P()
    {
        GameData.inst.Close_ServerClient();

        p2PLobbyMenu.Hide();
        p2p_Menu.Hide();
        main_Menu.Show();
        accountFastOverview_Menu.Show();
    }
    #endregion

    #region P2P Lobby
    public void Client_OpenLobby_OnConnect()
    {
        p2p_Menu.Hide();
        accountFastOverview_Menu.Hide();
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

    public void Button_P2P_Lobby_Back()
    {
        GameData.inst.Close_ServerClient();
        p2PLobbyMenu.Remove_AllPlayerPanels();
        p2PLobbyMenu.Hide();
        main_Menu.Show();
        accountFastOverview_Menu.Show();

        GameData.inst.account.isServer = false;
    }
    #endregion

    #region Offline Menu
    public void Button_Offline()
    {
        main_Menu.Hide();
        offlineMenu.Show();

        GameData.inst.CreateHost();
        GameData.inst.gameType = Utility.GameType.solo;
    }

    public void Button_Offline_Back()
    {
        offlineMenu.Hide();
        main_Menu.Show();
    }
    #endregion
}
