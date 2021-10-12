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

        health = new CharVars.char_Hp(25);
        movement = new CharVars.char_Move(4);

        defence.dodgeChance = 40;
        defence.blade_resistance = 0.3f;
        defence.pierce_resistance = 0.3f;
        defence.impact_resistance = 0.5f;
        defence.magic_resistance = 0.0f;

        CharAttack attack1 = new CharAttack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Blade;
        attack1.attacksCount = 2;
        attack1.attackDmg = 5;
        attacks.Add(attack1);

        CharAttack attack2 = new CharAttack();
        attack2.attackType = CharVars.attackType.Ranged;
        attack2.attackDmgType = CharVars.attackDmgType.Magic;
        attack2.attacksCount = 2;
        attack2.attackDmg = 5;
        attacks.Add(attack2);
    }
}
