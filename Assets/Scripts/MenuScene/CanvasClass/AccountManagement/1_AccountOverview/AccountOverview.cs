using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountOverview
{
    private AccountManagementMenu accountManagementMenu;

    private Button back_Button;

    public AccountOverview(AccountManagementMenu accountManagementMenu)
    {
        this.accountManagementMenu = accountManagementMenu;
        accountManagementMenu.overviewPanel_go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/OverviewPanel/Overview_Panel", typeof(GameObject)), accountManagementMenu.go.transform) as GameObject;

        this.back_Button = accountManagementMenu.overviewPanel_go.transform.Find("Back_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(Back);
    }

    public void Show()
    {
        accountManagementMenu.overviewPanel_go.SetActive(true);
    }

    private void Back()
    {
        accountManagementMenu.menuSceneMain.Button_AccountManagement_Back();
    }

    public void Hide()
    {
        accountManagementMenu.overviewPanel_go.SetActive(false);
    }
}
