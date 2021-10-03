using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattlePlayer
{
    public bool aiPlayer;

    public string name;
    public Hero hero;

    public List<Character> availableCharacters = new List<Character>();
    public List<Character> ingameCharacters = new List<Character>();

    public List<PlayerItem> items = new List<PlayerItem>();

    public BattlePlayer(Account acc, bool aiPlayer)
    {
        this.aiPlayer = aiPlayer;
        if(acc == null) this.name = "AI";
        else this.name = acc.name;
        if(aiPlayer) return;

        this.hero = acc.heroes[acc.battleHeroId];
        for(int x = 0; x < this.hero.battleCharacters.Count; x++)
        {
            Character c = this.hero.battleCharacters[x];
            this.availableCharacters.Add(c);
        }
        this.hero.battleCharacters.Clear();
        acc.heroes[acc.battleHeroId].battleCharacters.Clear();

        for(int x = 0; x < acc.items.Count; x++)
        {
            PlayerItem pi = acc.items[x];
            this.items.Add(pi);
        }
    }
}
