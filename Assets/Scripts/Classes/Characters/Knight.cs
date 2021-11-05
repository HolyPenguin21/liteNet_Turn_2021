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

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.3f;
        defence.pierce_resistance = 0.2f;
        defence.impact_resistance = 0.0f;
        defence.magic_resistance = 0.0f;

        health = new CharHealth(40);
        movement = new CharMove(4);

        CharAttack attack1 = 
            new CharAttack(CharVars.attackType.Melee, CharVars.attackDmgType.Blade, 2, 10);
        attacks.Add(attack1);
    }
}
