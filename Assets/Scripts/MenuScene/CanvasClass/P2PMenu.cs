using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P2PMenu : UI_Menu_Canvas
{
    private MenuSceneMain menuSceneMain;

    private Button host_Button;
    private InputField ip_Input;
    private Button client_Button;
    private Button back_Button;

    public P2PMenu (MenuSceneMain menuSceneMain)
    {
        this.menuSceneMain = menuSceneMain;
        go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/P2PPanel/P2P_Canvas", typeof(GameObject))) as GameObject;

        this.host_Button = go.transform.Find("Panel").transform.Find("Host_Button").GetComponent<Button>();
        this.host_Button.onClick.AddListener(menuSceneMain.Host);

        this.ip_Input = go.transform.Find("Panel").transform.Find("IP_InputField").GetComponent<InputField>();

        this.client_Button = go.transform.Find("Panel").transform.Find("Connect_Button").GetComponent<Button>();
        this.client_Button.onClick.AddListener(Client);

        this.back_Button = go.transform.Find("Panel").transform.Find("Back_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(menuSceneMain.Button_Back_P2P);
    }

    private void Client()
    {
        if(ip_Input.text == "") ip_Input.text = "127.0.0.1";
        GameData.inst.CreateClient(ip_Input.text);
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
