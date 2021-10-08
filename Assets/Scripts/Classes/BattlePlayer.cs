using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattlePlayer
{
    public bool aiPlayer;
    public Account account;

    public Character heroCharacter = null;
    public List<Character> availableCharacters = new List<Character>();
    public List<Character> ingameCharacters = new List<Character>();

    public BattlePlayer(Account account, bool aiPlayer)
    {
        this.aiPlayer = aiPlayer;

        if(this.aiPlayer) 
        {
            this.account = new Account();
            this.account.name = "Ai";
            return;
        }

        this.account = account;
        this.heroCharacter = account.heroes[account.battleHeroId].character;

        Setup_AvailableCharacters();
        Remove_Characters();
    }

    private void Setup_AvailableCharacters()
    {
        Hero hero = account.heroes[account.battleHeroId];

        for(int x = 0; x < hero.battleCharacters.Count; x++)
        {
            Character character = hero.battleCharacters[x];
            availableCharacters.Add(character);
        }
    }

    private void Remove_Characters()
    {
        account.heroes[account.battleHeroId].battleCharacters.Clear();
        
        if(account.name != GameData.inst.account.name) return;

        GameData.inst.account.heroes[account.battleHeroId].battleCharacters.Clear();
    }
}
