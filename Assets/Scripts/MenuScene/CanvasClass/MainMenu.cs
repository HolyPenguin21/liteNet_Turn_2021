using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UI_Menu_Canvas
{
    private MenuSceneMain menuSceneMain;

    private Button online_Button;
    private Button p2p_Button;
    private Button offline_Button;
    private Button settings_Button;
    private Button quit_Button;

    public MainMenu (MenuSceneMain menuSceneMain)
    {
        this.menuSceneMain = menuSceneMain;
        go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/MainMenuPanel/MainMenu_Canvas", typeof(GameObject))) as GameObject;

        this.online_Button = go.transform.Find("Panel").transform.Find("Online_Button").GetComponent<Button>();
        // this.online_Button.onClick.AddListener(menuSceneMain.AccountOverview_ManageAccount);
        this.online_Button.interactable = false;

        this.p2p_Button = go.transform.Find("Panel").transform.Find("P2P_Button").GetComponent<Button>();
        this.p2p_Button.onClick.AddListener(menuSceneMain.Button_P2P);

        this.offline_Button = go.transform.Find("Panel").transform.Find("Offine_Button").GetComponent<Button>();
        this.offline_Button.onClick.AddListener(menuSceneMain.Button_Offline);

        this.settings_Button = go.transform.Find("Panel").transform.Find("Settings_Button").GetComponent<Button>();
        // this.settings_Button.onClick.AddListener(Settings);
        this.settings_Button.interactable = false;

        this.quit_Button = go.transform.Find("Panel").transform.Find("Quit_Button").GetComponent<Button>();
        this.quit_Button.onClick.AddListener(Quit);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
