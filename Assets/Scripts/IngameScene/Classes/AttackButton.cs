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

    private AttackOrder attackOrder = new AttackOrder();

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

        CharVars.char_Attack a_Attack = attacker.attacks[a_attackId];
        button_Go.transform.Find("a_Image").GetComponent<Image>().sprite = Get_DmgTypeImage(a_Attack);
        button_Go.transform.Find("a_Text").GetComponent<Text>().text = 
        a_Attack.attackType + ", " + a_Attack.attackDmgType + ", " + a_Attack.attacksCount + "x" + attackOrder.DmgCalculation(a_Attack, target);

        CharVars.char_Attack t_Attack = Get_TargetAttack(a_Attack);
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
            t_Attack.attackType + ", " + t_Attack.attackDmgType + ", " + t_Attack.attacksCount + "x" + attackOrder.DmgCalculation(t_Attack, attacker);
            this.t_attackId = Get_TargetAttackId(target, t_Attack);
        }
    }

    private void TaskOnClick()
    {
        aPanel.SelectAttack(this.a_attackId, this.t_attackId);
    }
    
    private Sprite Get_DmgTypeImage(CharVars.char_Attack attack)
    {
        switch(attack.attackDmgType)
        {
            case CharVars.attackDmgType.Blade:
                return Resources.Load("Images/Blade", typeof(Sprite)) as Sprite;

            case CharVars.attackDmgType.Pierce:
                if(attack.attackType == CharVars.attackType.Melee)
                    return Resources.Load("Images/Pierce_meele", typeof(Sprite)) as Sprite;
                else
                    return Resources.Load("Images/Pierce_range", typeof(Sprite)) as Sprite;

            case CharVars.attackDmgType.Impact:
                return Resources.Load("Images/Impact", typeof(Sprite)) as Sprite;

            case CharVars.attackDmgType.Magic:
                return Resources.Load("Images/Magic", typeof(Sprite)) as Sprite;

            default :
                return Resources.Load("Images/Blade", typeof(Sprite)) as Sprite;
        }
    }

    private CharVars.char_Attack Get_TargetAttack(CharVars.char_Attack a_Attack)
    {
        List<CharVars.char_Attack> t_Attack_List = new List<CharVars.char_Attack>();
        for(int x = 0; x < target.attacks.Count; x++)
            if(target.attacks[x].attackType == a_Attack.attackType)
                t_Attack_List.Add(target.attacks[x]);

        CharVars.char_Attack t_Attack = new CharVars.char_Attack();
        t_Attack.attackType = CharVars.attackType.none;

        for(int x = 0; x < t_Attack_List.Count; x++)
        {
            int dmgCur = attackOrder.DmgCalculation(t_Attack, attacker);
            if(dmgCur < attackOrder.DmgCalculation(t_Attack_List[x], attacker))
                t_Attack = t_Attack_List[x];
        }

        return t_Attack;
    }

    private int Get_TargetAttackId(Character character, CharVars.char_Attack attack)
    {
        for(int x = 0; x < character.attacks.Count; x++)
            if(character.attacks[x].attackType == attack.attackType && character.attacks[x].attackDmg_cur == attack.attackDmg_cur)
                return x;

        return -1;
    }
}
