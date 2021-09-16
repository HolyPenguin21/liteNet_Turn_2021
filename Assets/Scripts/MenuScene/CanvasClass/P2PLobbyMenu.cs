using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P2PLobbyMenu : UI_Menu_Canvas
{
    private GameMain gameMain;
    public Text connectedPlayersList;
    public Button startGame_Button;

    public P2PLobbyMenu (GameObject gameObject)
    {
        go = gameObject;
        this.connectedPlayersList = go.transform.Find("Panel").transform.Find("Connected_Panel").transform.Find("PlayersList_Text").GetComponent<Text>();
        this.startGame_Button = go.transform.Find("Panel").transform.Find("StartGame_Button").GetComponent<Button>();

        this.startGame_Button.interactable = false;
    }

    public void UpdateConnectedPlayersList()
    {
        Server s = GameData.inst.server;
        Client c = GameData.inst.client;
        string result = "";
        connectedPlayersList.text = "";

        if(s != null)
        {
            for(int x = 0; x < s.players.Count; x++)
            {
                result += s.players[x].name + "\n";
            }
        }

        if(c != null)
        {
            for(int x = 0; x < c.players.Count; x++)
            {
                result += c.players[x].name + "\n";
            }
        }

        connectedPlayersList.text = result;
    }

    public void Add_PlayerPanel(int pos, Account account)
    {
        GameObject playerPanel_obj = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/Player_Panel", typeof(GameObject))) as GameObject;
        playerPanel_obj.transform.SetParent(go.transform.Find("Panel").transform, false);

        RectTransform playerPanel_rt = playerPanel_obj.GetComponent<RectTransform>();
        playerPanel_rt.anchorMin = new Vector2(0.284f, 0.88f - pos * 0.10f);
        playerPanel_rt.anchorMax = new Vector2(0.715f, 0.98f - pos * 0.10f);

        // Player name
        Text playerName = playerPanel_obj.transform.Find("PlayerName_Text").GetComponent<Text>();
        playerName.text = account.name;
        if(playerName.text != GameData.inst.account.name) 
            playerPanel_obj.transform.Find("HeroSelector_Dropdown").GetComponent<Dropdown>().interactable = false;

        // Hero change
        Dropdown heroDropdown = playerPanel_obj.transform.Find("HeroSelector_Dropdown").GetComponent<Dropdown>();
        heroDropdown.ClearOptions();
        List<string> heroDropdown_Options = new List<string>();
        for(int x = 0; x < account.heroes.Count; x++)
        {
            heroDropdown_Options.Add(account.heroes[x].name);
        }
        heroDropdown.AddOptions(heroDropdown_Options);
        heroDropdown.value = account.battleHeroId;
        heroDropdown.onValueChanged.AddListener(delegate {DropdownValueChanged(heroDropdown, playerName.text);});
    }
    
    void DropdownValueChanged(Dropdown dropdown, string pName)
    {
        if(pName != GameData.inst.account.name) return;
        if(gameMain == null) gameMain = GameData.inst.gameObject.GetComponent<GameMain>();

        Server s = GameData.inst.server;
        Client c = GameData.inst.client;

        Account p = null;

        if(s != null)
        {
            p = Utility.Get_Server_Player_ByName(pName);
            gameMain.Order_HeroChange(pName, dropdown.value);
        }

        if(c != null)
        {
            p = Utility.Get_Client_Player_ByName(pName);
            gameMain.Request_HeroChange(pName, dropdown.value);
        }
        // Debug.Log("Player " + p.pName + " has changed Hero to " + dropdown.value + " - " + p.pHeroes[dropdown.value].hName);
    }

    public void Remove_PlayerPanel(Account player)
    {
        var playerPanels = MonoBehaviour.FindObjectsOfType<Dropdown>();
        foreach(Dropdown d in playerPanels)
        {
            if(d.transform.parent.Find("PlayerName_Text").GetComponent<Text>().text == player.name)
            MonoBehaviour.Destroy(d.transform.parent.gameObject);
        }
    }

    public void Remove_AllPlayerPanels()
    {
        var playerPanels = MonoBehaviour.FindObjectsOfType<Dropdown>();
        foreach(Dropdown d in playerPanels)
        {
            MonoBehaviour.Destroy(d.transform.parent.gameObject);
        }
    }
}
