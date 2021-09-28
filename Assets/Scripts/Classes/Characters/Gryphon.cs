using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gryphon : Character
{
    public Gryphon(GameObject go_, Hex hex_, BattlePlayer owner_)
    {
        go = go_;
        if (go != null) tr = go_.transform;

        hex = hex_;
        owner = owner_;
        
        CharactersData cd = new CharactersData();
        // Edit
        id = 4;
        acc_cost = 40;
        ingame_cost = 10;
        name = "Gryphon";
        image = cd.Get_CharacterImage_ById(id);

        health = new CharVars.char_Hp(80);
        movement = new CharVars.char_Move(5);

        defence.dodgeChance = 30;
        defence.blade_resistance = 0.0f;
        defence.pierce_resistance = 0.0f;
        defence.impact_resistance = 0.0f;
        defence.magic_resistance = 0.0f;

        attacks = new List<CharVars.char_Attack>();
        CharVars.char_Attack attack1 = new CharVars.char_Attack();
        attack1.attackType = CharVars.attackType.Melee;
        attack1.attackDmgType = CharVars.attackDmgType.Blade;
        attack1.attacksCount = 2;
        attack1.attackDmg_base = 10;
        attack1.attackDmg_cur = attack1.attackDmg_base;
        attacks.Add(attack1);
    }
}
