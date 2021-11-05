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

        defence.dodgeChance = 30;
        defence.blade_resistance = -0.1f;
        defence.pierce_resistance = 0.0f;
        defence.impact_resistance = 0.3f;
        defence.magic_resistance = 0.0f;

        health = new CharHealth(16);
        movement = new CharMove(5);

        CharAttack attack1 = 
            new CharAttack(CharVars.attackType.Melee, CharVars.attackDmgType.Blade, 2, 5);
        attacks.Add(attack1);
    }
}
