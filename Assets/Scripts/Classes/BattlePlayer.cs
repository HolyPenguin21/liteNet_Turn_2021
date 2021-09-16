using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattlePlayer
{
    public bool aiPlayer;

    public string name;
    public Hero hero;

    public List<Character> availableCharacters;
    public List<Character> ingameCharacters;

    public List<PlayerItem> items;

    public BattlePlayer(Account acc, bool aiPlayer)
    {
        this.aiPlayer = aiPlayer;
        this.name = acc.name;
        this.availableCharacters = new List<Character>();
        this.ingameCharacters = new List<Character>();
        this.items = new List<PlayerItem>();

        if(aiPlayer) return;

        this.hero = acc.heroes[acc.battleHeroId];

        for(int x = 0; x < this.hero.battleCharacters.Count; x++)
        {
            Character c = this.hero.battleCharacters[x];
            this.availableCharacters.Add(c);
        }

        for(int x = 0; x < acc.items.Count; x++)
        {
            PlayerItem pi = acc.items[x];
            this.items.Add(pi);
        }
    }
}
