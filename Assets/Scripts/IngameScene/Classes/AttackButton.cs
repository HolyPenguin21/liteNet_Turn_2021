using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton
{
    private AttackPanel aPanel;
    private GameObject button_Go;
    public Button button;

    private Character attacker;
    private Character target;
    private int a_attackId;
    private int t_attackId;

    private AttackCalculation attackCalculation = new AttackCalculation();

    public AttackButton (AttackPanel aPanel, int a_attackId, Character attacker, Character target)
    {
        this.aPanel = aPanel;

        this.attacker = attacker;
        this.target = target;
        this.a_attackId = a_attackId;

        button_Go = MonoBehaviour.Instantiate(Resources.Load("UI_Ingame/Attack_Button", typeof(GameObject))) as GameObject;
        button_Go.name = "Attack " + this.a_attackId;
        button_Go.transform.SetParent(aPanel.aButton_Content.transform, false);

        button = button_Go.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        CharAttack a_Attack = attacker.attacks[a_attackId];
        button_Go.transform.Find("a_Image").GetComponent<Image>().sprite = Get_DmgTypeImage(a_Attack);
        button_Go.transform.Find("a_Text").GetComponent<Text>().text = 
        a_Attack.attackType + ", " + a_Attack.attackDmgType + ", " + a_Attack.attacksCount + "x" + attackCalculation.Hit_DmgCalculation(a_Attack, target);

        CharAttack t_Attack = attackCalculation.Get_ReturnAttack(attacker, a_attackId, target);
        if(t_Attack.attackType == CharVars.attackType.none)
        {
            button_Go.transform.Find("t_Image").gameObject.SetActive(false);
            button_Go.transform.Find("t_Text").gameObject.SetActive(false);
            this.t_attackId = -1;
        }
        else
        {
            button_Go.transform.Find("t_Image").GetComponent<Image>().sprite = Get_DmgTypeImage(t_Attack);
            button_Go.transform.Find("t_Text").GetComponent<Text>().text = 
            t_Attack.attackType + ", " + t_Attack.attackDmgType + ", " + t_Attack.attacksCount + "x" + attackCalculation.Hit_DmgCalculation(t_Attack, attacker);
            this.t_attackId = attackCalculation.Get_AttackId(target, t_Attack);
        }
    }

    public void TaskOnClick()
    {
        aPanel.SelectAttack(this.a_attackId, this.t_attackId);
    }
    
    private Sprite Get_DmgTypeImage(CharAttack attack)
    {
        switch(attack.attackDmgType)
        {
            case CharVars.attackDmgType.Blade:
                return Resources.Load("AttackTypes/Blade", typeof(Sprite)) as Sprite;

            case CharVars.attackDmgType.Pierce:
                if(attack.attackType == CharVars.attackType.Melee)
                    return Resources.Load("AttackTypes/Pierce_meele", typeof(Sprite)) as Sprite;
                else
                    return Resources.Load("AttackTypes/Pierce_range", typeof(Sprite)) as Sprite;

            case CharVars.attackDmgType.Impact:
                return Resources.Load("AttackTypes/Impact", typeof(Sprite)) as Sprite;

            case CharVars.attackDmgType.Magic:
                return Resources.Load("AttackTypes/Magic", typeof(Sprite)) as Sprite;

            default :
                return Resources.Load("AttackTypes/Blade", typeof(Sprite)) as Sprite;
        }
    }
}
