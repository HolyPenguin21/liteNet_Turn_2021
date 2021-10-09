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

        health = new CharVars.char_Hp(20);
        movement = new CharVars.char_Move(3);

        defence.dodgeChance = 20;
        defence.blade_resistance = 0.0f;
        defence.pierce_resistance = 0.2f;
        defence.impact_resistance = 0.1f;
        defence.magic_resistance = 0.0f;

        attacks = new List<CharVars.char_Attack>();
        CharVars.char_Attack attack2 = new CharVars.char_Attack();
        attack2.attackType = CharVars.attackType.Ranged;
        attack2.attackDmgType = CharVars.attackDmgType.Pierce;
        attack2.attacksCount = 3;
        attack2.attackDmg_base = 4;
        attack2.attackDmg_cur = attack2.attackDmg_base;
        attacks.Add(attack2);
    }
}
