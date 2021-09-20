using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPanel
{
    private GameObject canvas_Go;
    private List<AttackButton> attackButtons = new List<AttackButton>();

    public AttackPanel()
    {
        canvas_Go = GameObject.Find("Attack_Canvas");
        canvas_Go.SetActive(false);
    }

    public void Show(Character attacker, Character target)
    {
        if(!canvas_Go.activeInHierarchy) canvas_Go.SetActive(true);

        AttackButton aButton = new AttackButton(attacker.attacks[0], target.attacks[1]);
    }
}
