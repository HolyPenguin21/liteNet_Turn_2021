using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinLeader : Character
{
    public GoblinLeader()
    {
        CharactersData cd = new CharactersData();
        // Edit
        id = 7;
        acc_cost = 35;
        ingame_cost = 8;
        faction = CharVars.faction.Forest;
        name = "GoblinLeader";
        image = cd.Get_CharacterImage_ById(id);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.2f;
        defence.pierce_resistance = 0.1f;
        defence.impact_resistance = 0.2f;
        defence.magic_resistance = 0.1f;

        health = new CharHealth(60);
        movement = new CharMove(3);

        CharAttack attack1 = 
            new CharAttack(CharVars.attackType.Melee, CharVars.attackDmgType.Pierce, 2, 8);
        attacks.Add(attack1);

        CharAttack attack2 = 
            new CharAttack(CharVars.attackType.Ranged, CharVars.attackDmgType.Pierce, 2, 6);
        attacks.Add(attack2);
    }
}
