using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P2PMenu : UI_Menu_Canvas
{
    public Button host_Button;
    public Button client_Button;

    public P2PMenu (GameObject gameObject)
    {
        go = gameObject;

        host_Button = go.transform.Find("Panel").transform.Find("Host_Button").GetComponent<Button>();
        client_Button = go.transform.Find("Panel").transform.Find("Connect_Button").GetComponent<Button>();
    }

    public void CheckPlayer()
    {
        host_Button.interactable = true;
        client_Button.interactable = true;

        Account acc = GameData.inst.account;
        if(acc == null)
        {
            host_Button.interactable = false;
            client_Button.interactable = false;
            return;
        }

        if(acc.heroes.Count == 0 || acc.heroes[acc.battleHeroId].battleCharacters.Count == 0)
        {
            host_Button.interactable = false;
            client_Button.interactable = false;
            return;
        }
    }
}
