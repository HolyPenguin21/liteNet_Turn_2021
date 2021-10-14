using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Character
{
    public Knight()
    {
        CharactersData cd = new CharactersData();
        // Edit
        id = 3;
        acc_cost = 25;
        ingame_cost = 7;
        faction = CharVars.faction.Castle;
        name = "Knight";
        image = cd.Get_CharacterImage_ById(id);

        health = new CharVars.char_Hp(40);
        movement = new CharVars.char_Move(4);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.3f;
        defence.pierce_resistance = 0.2f;
        defence.impact_resistance = 0.0f;
        defence.magic_resistance = 0.0f;

        CharAttack attack1 = new CharAttack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Blade;
        attack1.attacksCount = 2;
        attack1.attackDmg = 10;
        attacks.Add(attack1);
    }
}
