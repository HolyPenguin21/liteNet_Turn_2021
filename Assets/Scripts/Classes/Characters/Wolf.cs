using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Character
{
    public Wolf()
    {
        CharactersData cd = new CharactersData();
        // Edit
        id = 5;
        acc_cost = 8;
        ingame_cost = 2;
        faction = CharVars.faction.Forest;
        name = "Wolf";
        image = cd.Get_CharacterImage_ById(id);

        health = new CharVars.char_Hp(16);
        movement = new CharVars.char_Move(5);

        defence.dodgeChance = 30;
        defence.blade_resistance = -0.1f;
        defence.pierce_resistance = 0.0f;
        defence.impact_resistance = 0.3f;
        defence.magic_resistance = 0.0f;

        CharAttack attack1 = new CharAttack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Blade;
        attack1.attacksCount = 2;
        attack1.attackDmg = 5;
        attacks.Add(attack1);
    }
}
