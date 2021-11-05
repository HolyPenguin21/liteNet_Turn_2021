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
        faction = CharVars.faction.General;
        name = "Swordman";
        image = cd.Get_CharacterImage_ById(id);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.2f;
        defence.pierce_resistance = 0.1f;
        defence.impact_resistance = 0.0f;
        defence.magic_resistance = 0.0f;

        health = new CharHealth(30);
        movement = new CharMove(4);

        CharAttack attack1 = 
            new CharAttack(CharVars.attackType.Melee, CharVars.attackDmgType.Blade, 3, 5);
        attacks.Add(attack1);
    }
}
