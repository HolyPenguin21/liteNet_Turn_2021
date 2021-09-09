using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero
{
    public Character character;

    public string name;
    public List<Character> battleCharacters;

    public Hero(Character character, string name)
    {
        this.character = character;
        this.name = name;
        
        battleCharacters = new List<Character>();
    }
}
