using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCalculation
{
    public CharVars.char_Attack Get_MaxDmgAttack(Character a_character, Character t_character)
    {
        CharVars.char_Attack a_attack = new CharVars.char_Attack();

        int curMaxDmg = 0;
        for (int x = 0; x < a_character.attacks.Count; x++)
        {
            CharVars.char_Attack tempAttack = a_character.attacks[x];
            int dmg = tempAttack.attacksCount * Hit_DmgCalculation(tempAttack, t_character);
            if(dmg > curMaxDmg)
            {
                curMaxDmg = dmg;
                a_attack = tempAttack;
            }
        }

        return a_attack;
    }

    public int Hit_DmgCalculation(CharVars.char_Attack a_Attack, Character target)
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

    public CharVars.char_Attack Get_ReturnAttack(Character a_Character, int a_AttackId, Character t_Character)
    {
        List<CharVars.char_Attack> t_Attack_List = new List<CharVars.char_Attack>();

        for(int x = 0; x < t_Character.attacks.Count; x++)
        {
            CharVars.char_Attack someAttack = t_Character.attacks[x];
            
            if(someAttack.attackType == a_Character.attacks[a_AttackId].attackType)
                t_Attack_List.Add(someAttack);
        }

        CharVars.char_Attack t_Attack = new CharVars.char_Attack();
        t_Attack.attackType = CharVars.attackType.none;

        for(int x = 0; x < t_Attack_List.Count; x++)
        {
            CharVars.char_Attack someAttack = t_Attack_List[x];

            int dmgCur = someAttack.attacksCount * Hit_DmgCalculation(someAttack, a_Character);

            if(dmgCur > t_Attack.attacksCount * Hit_DmgCalculation(t_Attack, a_Character))
                t_Attack = someAttack;
        }

        return t_Attack;
    }

    public int Get_AttackId(Character character, CharVars.char_Attack attack)
    {
        for(int x = 0; x < character.attacks.Count; x++)
        {
            CharVars.char_Attack someAttack = character.attacks[x];

            if(someAttack.attackType == attack.attackType &&
            someAttack.attackDmgType == attack.attackDmgType &&
            someAttack.attacksCount == attack.attacksCount &&
            someAttack.attackDmg_cur == attack.attackDmg_cur)
                return x;
        }

        return -1;
    }
}
