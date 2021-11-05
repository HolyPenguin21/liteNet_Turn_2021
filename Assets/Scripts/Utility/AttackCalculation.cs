using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCalculation
{
    public CharAttack Get_MaxDmgAttack(Character a_character, Character t_character)
    {
        CharAttack a_attack = new CharAttack(CharVars.attackType.none, CharVars.attackDmgType.none, 0, 0);

        int curMaxDmg = 0;
        for (int x = 0; x < a_character.attacks.Count; x++)
        {
            CharAttack tempAttack = a_character.attacks[x];
            int dmg = tempAttack.attacksCount_cur * Hit_DmgCalculation(tempAttack, t_character);
            if(dmg > curMaxDmg)
            {
                curMaxDmg = dmg;
                a_attack = tempAttack;
            }
        }

        return a_attack;
    }

    public int Hit_DmgCalculation(CharAttack a_Attack, Character target)
    {
        CharVars.char_Defence t_Defence = target.defence;

        int dmg = a_Attack.attackDmg_cur;

        switch (a_Attack.attackDmgType)
        {
            case CharVars.attackDmgType.Blade:
                return Convert.ToInt32(dmg - dmg * t_Defence.blade_resistance);
            case CharVars.attackDmgType.Pierce:
                return Convert.ToInt32(dmg - dmg * t_Defence.pierce_resistance);
            case CharVars.attackDmgType.Impact:
                return Convert.ToInt32(dmg - dmg * t_Defence.impact_resistance);
            case CharVars.attackDmgType.Magic:
                return Convert.ToInt32(dmg - dmg * t_Defence.magic_resistance);
        }

        return -999; // should not get here
    }

    public int Hit_Trauma(CharAttack a_Attack, Character target)
    {
        LifetimeBuff debuff = null;

        switch (a_Attack.attackDmgType)
        {
            case CharVars.attackDmgType.Blade:
                debuff = new ChestWound();
            break;

            case CharVars.attackDmgType.Pierce:
                debuff = new ChestWound();
            break;

            case CharVars.attackDmgType.Impact:
                debuff = new BrokenLeg();
            break;

            case CharVars.attackDmgType.Magic:

            break;
        }

        return debuff.id;
    }

    public CharAttack Get_ReturnAttack(Character a_Character, int a_AttackId, Character t_Character)
    {
        List<CharAttack> t_Attack_List = new List<CharAttack>();

        for(int x = 0; x < t_Character.attacks.Count; x++)
        {
            CharAttack someAttack = t_Character.attacks[x];
            
            if(someAttack.attackType == a_Character.attacks[a_AttackId].attackType)
                t_Attack_List.Add(someAttack);
        }

        CharAttack t_Attack = new CharAttack(CharVars.attackType.none, CharVars.attackDmgType.none, 0, 0);

        for(int x = 0; x < t_Attack_List.Count; x++)
        {
            CharAttack someAttack = t_Attack_List[x];

            int dmgCur = someAttack.attacksCount_cur * Hit_DmgCalculation(someAttack, a_Character);

            if(dmgCur > t_Attack.attacksCount_cur * Hit_DmgCalculation(t_Attack, a_Character))
                t_Attack = someAttack;
        }

        return t_Attack;
    }

    public int Get_AttackId(Character character, CharAttack attack)
    {
        for(int x = 0; x < character.attacks.Count; x++)
        {
            CharAttack someAttack = character.attacks[x];

            if(someAttack.attackType == attack.attackType &&
            someAttack.attackDmgType == attack.attackDmgType &&
            someAttack.attacksCount_cur == attack.attacksCount_cur &&
            someAttack.attackDmg_cur == attack.attackDmg_cur)
                return x;
        }

        return -1;
    }
}
