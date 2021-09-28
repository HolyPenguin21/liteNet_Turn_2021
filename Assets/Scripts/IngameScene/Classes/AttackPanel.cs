using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPanel
{
    private SceneMain_UI sceneUI;

    private GameObject canvas_Go;
    public GameObject aButton_Content;
    private Button confirm_Button;
    private Button cancel_Button;

    private Character attacker;
    private Character target;
    private List<AttackButton> attackButtons = new List<AttackButton>();

    private int a_AttackId = 0;
    private int t_AttackId = 0;

    public AttackPanel(SceneMain_UI sceneUI)
    {
        this.sceneUI = sceneUI;
        canvas_Go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Attack_Canvas", typeof(GameObject))) as GameObject;

        aButton_Content = canvas_Go.transform.Find("Panel").transform.Find("Attack_ScrollView").transform.Find("Viewport").transform.Find("Content").gameObject;

        confirm_Button = canvas_Go.transform.Find("Panel").transform.Find("Confirm_Button").GetComponent<Button>();
        confirm_Button.onClick.AddListener(Confirm_Attack);
        
        cancel_Button = canvas_Go.transform.Find("Panel").transform.Find("Cancel_Button").GetComponent<Button>();
        cancel_Button.onClick.AddListener(Hide);

        Hide();
    }

    public void Show(Character attacker, Character target)
    {
        if(!canvas_Go.activeInHierarchy) canvas_Go.SetActive(true);
        sceneUI.mouseOverUI = true;

        this.attacker = attacker;
        this.target = target;

        // Clear
        attackButtons.Clear();
        foreach (Transform child in aButton_Content.transform)
            GameObject.Destroy(child.gameObject);

        for(int x = 0; x < attacker.attacks.Count; x++)
        {
            AttackButton aButton = new AttackButton(this, x, attacker, target);
            attackButtons.Add(aButton);
        }
    }

    public void SelectAttack(int a_AttackId, int t_AttackId)
    {
        this.a_AttackId = a_AttackId;
        this.t_AttackId = t_AttackId;

        for(int x = 0; x < attackButtons.Count; x++)
        {
            Button aButton = attackButtons[x].button;
            if(x == this.a_AttackId)
                aButton.interactable = false;
            else
                aButton.interactable = true;
        }
    }

    private void Confirm_Attack()
    {
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;

        if(server != null)
        {
            GameData.inst.gameMain.Order_Attack(attacker.hex, this.a_AttackId, target.hex, this.t_AttackId);
        }
        else
        {
            GameData.inst.gameMain.Request_Attack(attacker.hex, this.a_AttackId, target.hex, this.t_AttackId);
        }

        Hide();
    }

    public void Hide()
    {
        if(canvas_Go.activeInHierarchy) canvas_Go.SetActive(false);
        sceneUI.mouseOverUI = false;
    }
}
