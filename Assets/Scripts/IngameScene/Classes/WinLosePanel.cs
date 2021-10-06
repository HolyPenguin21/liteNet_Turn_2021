using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLosePanel
{
    private SceneMain_UI sceneUI;
    private SceneMain sceneMain;

    public GameObject canvas_Go;
    public GameObject reward_Content;

    public Text winLose_Text;
    private Button ok_Button;

    private List<RewardItem> rewards = new List<RewardItem>();

    private Account winner;
    private Account loser;

    public WinLosePanel(SceneMain_UI sceneUI, SceneMain sceneMain)
    {
        this.sceneUI = sceneUI;
        this.sceneMain = sceneMain;

        canvas_Go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/WinLose_Canvas", typeof(GameObject))) as GameObject;
        reward_Content = canvas_Go.transform.Find("Panel").transform.Find("Reward_ScrollView").transform.Find("Viewport").transform.Find("Content").gameObject;

        winLose_Text = canvas_Go.transform.Find("Panel").transform.Find("WinLose_Text").GetComponent<Text>();
        ok_Button = canvas_Go.transform.Find("Panel").transform.Find("Ok_Button").GetComponent<Button>();
        ok_Button.onClick.AddListener(Confirm);

        Hide();
    }

    public void Show(BattlePlayer winner, string rewardsList)
    {
        if(!canvas_Go.activeInHierarchy) canvas_Go.SetActive(true);
        sceneUI.mouseOverUI = true;

        // Clear
        rewards.Clear();
        foreach (Transform child in reward_Content.transform)
            GameObject.Destroy(child.gameObject);

        winLose_Text.text = winner.account.name + " has win the game. His reward :";

        PlayerItemsData playerItemsData = new PlayerItemsData();
        if(rewardsList == "") return;
        string[] rewardsData = rewardsList.Split(',');
        for(int x = 0; x < rewardsData.Length; x++)
        {
            PlayerItem playerItem = playerItemsData.Get_PlayerItem_ById(Convert.ToInt32(rewardsData[x]));
            RewardItem rewardItem = new RewardItem(this, playerItem);
            rewards.Add(rewardItem);
        }
    }

    private void Confirm()
    {
        GameData.inst.Close_ServerClient();
        Utility.Load_Scene(0);

        // add rewards
    }

    public void Hide()
    {
        if(canvas_Go.activeInHierarchy) canvas_Go.SetActive(false);
        sceneUI.mouseOverUI = false;
    }
}
