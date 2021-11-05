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

        defence.dodgeChance = 30;
        defence.blade_resistance = 0.3f;
        defence.pierce_resistance = -0.2f;
        defence.impact_resistance = 0.3f;
        defence.magic_resistance = 0.3f;

        health = new CharHealth(80);
        movement = new CharMove(5);

        CharAttack attack1 = 
            new CharAttack(CharVars.attackType.Melee, CharVars.attackDmgType.Blade, 2, 12);
        attacks.Add(attack1);
    }
}
