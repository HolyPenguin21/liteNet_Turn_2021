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

        health = new CharVars.char_Hp(30);
        movement = new CharVars.char_Move(4);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.0f;
        defence.pierce_resistance = 0.2f;
        defence.impact_resistance = 0.0f;
        defence.magic_resistance = 0.0f;

        CharAttack attack1 = new CharAttack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Pierce;
        attack1.attacksCount = 2;
        attack1.attackDmg = 5;
        attacks.Add(attack1);

        CharAttack attack2 = new CharAttack();
        attack2.attackType = CharVars.attackType.Ranged;
        attack2.attackDmgType = CharVars.attackDmgType.Pierce;
        attack2.attacksCount = 1;
        attack2.attackDmg = 5;
        attacks.Add(attack2);
    }
}
