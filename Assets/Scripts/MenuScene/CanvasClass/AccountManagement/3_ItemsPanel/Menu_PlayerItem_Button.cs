using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_PlayerItem_Button : MonoBehaviour
{
    private ItemsPanel itemsPanel;

    private Account account;
    private PlayerItem item;

    private Button button;
    private Image itemImage;
    private Text itemText;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
    }

    public void Init(ItemsPanel itemsPanel, Account account, PlayerItem item)
    {
        this.itemsPanel = itemsPanel;
        this.account = account;
        this.item = item;

        this.itemImage = GetComponent<Image>();
        this.itemImage.sprite = item.image;

        this.itemText = transform.Find("Text").GetComponent<Text>();
        this.itemText.text = item.name;
    }

    public void TaskOnClick()
    {
        item.Effect();

        if(item.oneTime)
        {
            account.items.Remove(item);
            Destroy(gameObject);
        }

        itemsPanel.Show();
        itemsPanel.accountManagementMenu.Update_AccountManagementMenu();
	}
}
