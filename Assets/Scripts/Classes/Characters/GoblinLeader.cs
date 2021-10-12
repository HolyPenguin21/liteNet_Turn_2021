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

        health = new CharVars.char_Hp(60);
        movement = new CharVars.char_Move(3);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.2f;
        defence.pierce_resistance = 0.1f;
        defence.impact_resistance = 0.2f;
        defence.magic_resistance = 0.1f;

        CharAttack attack1 = new CharAttack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Pierce;
        attack1.attacksCount = 2;
        attack1.attackDmg = 8;
        attacks.Add(attack1);

        CharAttack attack2 = new CharAttack();
        attack2.attackType = CharVars.attackType.Ranged;
        attack2.attackDmgType = CharVars.attackDmgType.Pierce;
        attack2.attacksCount = 2;
        attack2.attackDmg = 6;
        attacks.Add(attack2);
    }
}
