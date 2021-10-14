using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineMenu : UI_Menu_Canvas
{
    private MenuSceneMain menuSceneMain;

    private Button event_Wolf_Button;
    private Button event_Goblin_Button;

    private Button back_Button;

    public OfflineMenu (MenuSceneMain menuSceneMain)
    {
        this.menuSceneMain = menuSceneMain;
        go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/Offine_Canvas", typeof(GameObject))) as GameObject;

        this.event_Wolf_Button = go.transform.Find("Panel").transform.Find("WolfEvent_Button").GetComponent<Button>();
        this.event_Wolf_Button.onClick.AddListener(Button_WolfEvent);

        this.event_Goblin_Button = go.transform.Find("Panel").transform.Find("GoblinEvent_Button").GetComponent<Button>();
        this.event_Goblin_Button.onClick.AddListener(Button_GoblinEvent);

        this.back_Button = go.transform.Find("Panel").transform.Find("Back_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(menuSceneMain.Button_Offline_Back);
    }

    public void Button_WolfEvent()
    {
        GameData.inst.gameMain.Order_StartGame(2);
    }

    public void Button_GoblinEvent()
    {
        GameData.inst.gameMain.Order_StartGame(3);
    }
}
