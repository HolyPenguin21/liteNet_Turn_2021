using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPanel
{
    public AccountManagementMenu accountManagementMenu;

    private GameObject panel_Content;

    private Button back_Button;

    public ItemsPanel(AccountManagementMenu accountManagementMenu)
    {
        this.accountManagementMenu = accountManagementMenu;
        accountManagementMenu.itemsPanel_go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/ItemsPanel/Items_Panel", typeof(GameObject)), accountManagementMenu.go.transform) as GameObject;

        this.panel_Content = accountManagementMenu.itemsPanel_go.transform.Find("Items_View").transform.Find("Viewport").transform.Find("Content").gameObject;

        this.back_Button = accountManagementMenu.itemsPanel_go.transform.Find("Back_Button").GetComponent<Button>();
        this.back_Button.onClick.AddListener(Back);

        Hide();
    }

    public void Show()
    {
        // Clear
        foreach (Transform child in panel_Content.transform)
            GameObject.Destroy(child.gameObject);

        accountManagementMenu.itemsPanel_go.SetActive(true);

        for(int x = 0; x < GameData.inst.account.items.Count; x++)
        {
            PlayerItem item = GameData.inst.account.items[x];
            GameObject button_go = MonoBehaviour.Instantiate(Resources.Load("UI_MainMenu/AccountPanel/ItemsPanel/PlayerItem_Button", typeof(GameObject)), panel_Content.transform) as GameObject;
            Menu_PlayerItem_Button playerItem_Button = button_go.GetComponent<Menu_PlayerItem_Button>();
            playerItem_Button.Init(this, GameData.inst.account, item);
        }
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
