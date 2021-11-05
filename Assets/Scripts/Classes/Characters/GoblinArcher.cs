using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinArcher : Character
{
    public GoblinArcher()
    {
        CharactersData cd = new CharactersData();
        // Edit
        id = 8;
        acc_cost = 8;
        ingame_cost = 2;
        faction = CharVars.faction.Forest;
        name = "GoblinArcher";
        image = cd.Get_CharacterImage_ById(id);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.0f;
        defence.pierce_resistance = 0.2f;
        defence.impact_resistance = 0.1f;
        defence.magic_resistance = 0.0f;

        health = new CharHealth(20);
        movement = new CharMove(3);

        CharAttack attack2 = 
            new CharAttack(CharVars.attackType.Ranged, CharVars.attackDmgType.Pierce, 3, 4);
        attacks.Add(attack2);
    }
}
