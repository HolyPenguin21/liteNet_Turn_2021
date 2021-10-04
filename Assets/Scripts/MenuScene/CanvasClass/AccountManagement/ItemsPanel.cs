using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPanel
{
    private AccountManagementMenu accountManagementMenu;

    private Button back_Button;

    public ItemsPanel(AccountManagementMenu accountManagementMenu)
    {
        this.accountManagementMenu = accountManagementMenu;
        accountManagementMenu.itemsPanel_go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/Items_Panel", typeof(GameObject)), accountManagementMenu.go.transform) as GameObject;

        this.back_Button = accountManagementMenu.itemsPanel_go.transform.Find("Back_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(Back);

        Hide();
    }

    public void Show()
    {
        accountManagementMenu.itemsPanel_go.SetActive(true);
    }

    private void Back()
    {
        accountManagementMenu.menuSceneMain.Button_AccountManagement_Back();
    }

    public void Hide()
    {
        accountManagementMenu.itemsPanel_go.SetActive(false);
    }
}
