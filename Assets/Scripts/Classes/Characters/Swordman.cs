using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : Character
{
    public Swordman()
    {
        CharactersData cd = new CharactersData();
        // Edit
        id = 1;
        acc_cost = 15;
        ingame_cost = 3;
        name = "Swordman";
        image = cd.Get_CharacterImage_ById(id);

        health = new CharVars.char_Hp(30);
        movement = new CharVars.char_Move(4);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.0f;
        defence.pierce_resistance = 0.0f;
        defence.impact_resistance = 0.0f;
        defence.magic_resistance = 0.0f;

        attacks = new List<CharVars.char_Attack>();
        CharVars.char_Attack attack1 = new CharVars.char_Attack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Blade;
        attack1.attacksCount = 3;
        attack1.attackDmg_base = 5;
        attack1.attackDmg_cur = attack1.attackDmg_base;
        attacks.Add(attack1);
    }
}
