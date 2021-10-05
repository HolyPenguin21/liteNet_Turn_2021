using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountFastOverview : UI_Menu_Canvas
{
    private MenuSceneMain menuSceneMain;

    private Text pInfo_Text;
    private Button manageAccount_Button;
    private Button back_Button;

    public AccountFastOverview (MenuSceneMain menuSceneMain)
    {
        this.menuSceneMain = menuSceneMain;
        go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountFastOverview_Canvas", typeof(GameObject))) as GameObject;

        this.pInfo_Text = go.transform.Find("Panel").transform.Find("pInfo_Text").GetComponent<Text>();
        
        this.manageAccount_Button = go.transform.Find("Panel").transform.Find("Manage_Button").GetComponent<Button>();
        this.manageAccount_Button.onClick.AddListener(menuSceneMain.AccountOverview_ManageAccount);
        
        this.back_Button = go.transform.Find("Panel").transform.Find("Back_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(menuSceneMain.AccountOverview_Back);
    }

    public void Update_PlayerInfo()
    {
        Account account = GameData.inst.account;

        string result = "";
        result += account.name + "\n";
        result += "Gold : " + account.acc_gold + "\n\n";

        if(account.heroes.Count != 0)
        {
            Hero hero = account.heroes[account.battleHeroId];
            result +="Selected hero :\n - " + hero.name + "\n";

            result += "Selected characters :\n";
            for(int x = 0; x < hero.battleCharacters.Count; x++)
            {
                result += " - " + hero.battleCharacters[x].name + "\n";
            }
            result = result.Remove(result.Length - 1);
        }
        else
        {
            result += "Selected hero :\n - Not selected" + "\n\n";
            result += "Selected characters :\n - Not selected";
        }

        pInfo_Text.text = result;
    }
}
