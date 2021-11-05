using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Character
{
    public Spearman()
    {
        CharactersData cd = new CharactersData();
        // Edit
        id = 2;
        acc_cost = 10;
        ingame_cost = 2;
        faction = CharVars.faction.General;
        name = "Spearman";
        image = cd.Get_CharacterImage_ById(id);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.0f;
        defence.pierce_resistance = 0.2f;
        defence.impact_resistance = 0.0f;
        defence.magic_resistance = 0.0f;

        health = new CharHealth(30);
        movement = new CharMove(4);

        CharAttack attack1 = 
            new CharAttack(CharVars.attackType.Melee, CharVars.attackDmgType.Pierce, 2, 5);
        attacks.Add(attack1);

        CharAttack attack2 = 
            new CharAttack(CharVars.attackType.Ranged, CharVars.attackDmgType.Pierce, 1, 5);
        attacks.Add(attack2);
    }
}
