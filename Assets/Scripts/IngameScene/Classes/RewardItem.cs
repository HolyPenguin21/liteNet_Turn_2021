using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem
{
    private WinLosePanel winLosePanel;
    private GameObject item_Go;

    public RewardItem(WinLosePanel winLosePanel, PlayerItem playerItem)
    {
        this.winLosePanel = winLosePanel;

        item_Go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Reward_Image", typeof(GameObject))) as GameObject;
        item_Go.transform.SetParent(winLosePanel.reward_Content.transform, false);

        item_Go.GetComponent<Image>().sprite = playerItem.image;
    }
}
