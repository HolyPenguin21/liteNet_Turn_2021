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

        health = new CharVars.char_Hp(30);
        movement = new CharVars.char_Move(4);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.2f;
        defence.pierce_resistance = 0.1f;
        defence.impact_resistance = 0.0f;
        defence.magic_resistance = 0.0f;

        CharAttack attack1 = new CharAttack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Blade;
        attack1.attacksCount = 3;
        attack1.attackDmg = 5;
        attacks.Add(attack1);
    }
}
