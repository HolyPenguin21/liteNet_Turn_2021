using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gryphon : Character
{
    public Gryphon()
    {
        CharactersData cd = new CharactersData();
        // Edit
        id = 4;
        acc_cost = 40;
        ingame_cost = 10;
        faction = CharVars.faction.Forest;
        name = "Gryphon";
        image = cd.Get_CharacterImage_ById(id);

        health = new CharVars.char_Hp(80);
        movement = new CharVars.char_Move(5);

        defence.dodgeChance = 30;
        defence.blade_resistance = 0.3f;
        defence.pierce_resistance = -0.2f;
        defence.impact_resistance = 0.3f;
        defence.magic_resistance = 0.3f;

        CharAttack attack1 = new CharAttack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Blade;
        attack1.attacksCount = 2;
        attack1.attackDmg = 12;
        attacks.Add(attack1);
    }
}
