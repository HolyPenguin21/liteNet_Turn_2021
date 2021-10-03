using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Character
{
    public Shadow()
    {
        CharactersData cd = new CharactersData();
        // Edit
        id = 6;
        acc_cost = 20;
        ingame_cost = 5;
        name = "Shadow";
        image = cd.Get_CharacterImage_ById(id);

        health = new CharVars.char_Hp(25);
        movement = new CharVars.char_Move(4);

        defence.dodgeChance = 40;
        defence.blade_resistance = 0.0f;
        defence.pierce_resistance = 0.0f;
        defence.impact_resistance = 0.0f;
        defence.magic_resistance = 0.0f;

        attacks = new List<CharVars.char_Attack>();
        CharVars.char_Attack attack1 = new CharVars.char_Attack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Blade;
        attack1.attacksCount = 2;
        attack1.attackDmg_base = 5;
        attack1.attackDmg_cur = attack1.attackDmg_base;
        attacks.Add(attack1);

        CharVars.char_Attack attack2 = new CharVars.char_Attack();
        attack2.attackType = CharVars.attackType.Ranged;
        attack2.attackDmgType = CharVars.attackDmgType.Magic;
        attack2.attacksCount = 2;
        attack2.attackDmg_base = 5;
        attack2.attackDmg_cur = attack2.attackDmg_base;
        attacks.Add(attack2);
    }
}
