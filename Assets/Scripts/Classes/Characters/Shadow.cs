using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Character
{
    public Shadow()
    {
        CharactersData cd = new CharactersData();
        // Edit
        id = 6;
        acc_cost = 20;
        ingame_cost = 5;
        faction = CharVars.faction.Dark;
        name = "Shadow";
        image = cd.Get_CharacterImage_ById(id);

        defence.dodgeChance = 40;
        defence.blade_resistance = 0.3f;
        defence.pierce_resistance = 0.3f;
        defence.impact_resistance = 0.5f;
        defence.magic_resistance = 0.0f;

        health = new CharHealth(25);
        movement = new CharMove(4);

        CharAttack attack1 = 
            new CharAttack(CharVars.attackType.Melee, CharVars.attackDmgType.Blade, 2, 5);
        attacks.Add(attack1);

        CharAttack attack2 = 
            new CharAttack(CharVars.attackType.Ranged, CharVars.attackDmgType.Magic, 2, 5);
        attacks.Add(attack2);
    }
}
