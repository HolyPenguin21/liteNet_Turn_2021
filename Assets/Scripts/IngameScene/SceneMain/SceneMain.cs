using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMain : MonoBehaviour
{
    private GameMain gameMain;
    public Utility.EventType eventType;
    public Hex[] grid;
    public List<Hex> startPoints = new List<Hex>();
    public List<Hex> bossSpawners = new List<Hex>();

    public BattlePlayer battlePlayer; // my battle player
    public List<BattlePlayer> battlePlayers_List = new List<BattlePlayer>(); // list of all battle players
    public BattlePlayer currentTurn; // current turn for this battle player

    public Utility.GameType gameType;
    public AiBehaviour aiBehaviour;
    public List<PlayerItem> rewards = new List<PlayerItem>();

    // UI
    public Text currentTurn_Text;
    public Button endTurn_Button;

    private void Awake()
    {
        // UI
        currentTurn_Text = GameObject.Find("CurrentTurn_Text").GetComponent<Text>();
        endTurn_Button = GameObject.Find("EndTurn_Button").GetComponent<Button>();
        endTurn_Button.interactable = false;

        gameMain = GameData.inst.gameMain;
        gameMain.sceneMain = this;

        Utility.Set_InputType();
    }

    private IEnumerator Start()
    {
        Get_GameType(); // both

        Server server = GameData.inst.server;
        if(server == null) yield break;

        yield return new WaitForSeconds(0.5f);
        gameMain.Order_SetupPvpPlayers(); // server with order

        yield return new WaitForSeconds(0.5f);
        Setup_HeroCharacters(); // server with order
        Setup_Ai(); // server local

        yield return new WaitForSeconds(0.5f);
        Setup_FirstTurn(); // server with order
    }

    #region Turn management
    public void Setup_FirstTurn()
    {
        int bpId = 0;
        if(gameType == Utility.GameType.solo)
        {
            
        }
        else if(gameType == Utility.GameType.pvp)
        {
            bpId = UnityEngine.Random.Range(0, battlePlayers_List.Count);
            Setup_Reward();
        }

        gameMain.Order_SetTurn(bpId);
    }

    public void Button_EndTurn()
    {
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;

        if(server != null) {
            int bpId = battlePlayers_List.IndexOf(currentTurn);
            bpId++;
            if(bpId >= battlePlayers_List.Count) bpId = 0;
            gameMain.Order_SetTurn(bpId);
        }
        else {
            gameMain.Request_EndTurn();
        }
    }

    // Will be called on both Host and Client
    public IEnumerator On_TurnChange()
    {
        currentTurn_Text.text = "Current turn for : " + currentTurn.account.name;

        if(battlePlayer == currentTurn) endTurn_Button.interactable = true;
        else endTurn_Button.interactable = false;

        for(int x = 0; x < battlePlayers_List.Count; x++) 
        {
            BattlePlayer bp = battlePlayers_List[x];
            if(bp == currentTurn)
            {
                // Hero character
                if(bp.heroCharacter != null)
                {
                    Character heroCharacter = bp.heroCharacter;
                    Character_CanAct(heroCharacter);
                }
                //Ingame characters
                for(int y = 0; y < bp.ingameCharacters.Count; y++) 
                {
                    Character character = bp.ingameCharacters[y];
                    Character_CanAct(character);
                }
            }
            else
            {
                // Hero character
                if(bp.heroCharacter != null)
                {
                    Character heroCharacter = bp.heroCharacter;
                    Character_CantAct(heroCharacter);
                }
                //Ingame characters
                for(int y = 0; y < bp.ingameCharacters.Count; y++) 
                {
                    Character character = bp.ingameCharacters[y];
                    Character_CantAct(character);
                }
            }
        }

        if(GameData.inst.server != null && currentTurn.aiPlayer)
            StartCoroutine(aiBehaviour.AITurn());

        yield return null;
    }
    private void Character_CanAct(Character character)
    {
        character.canAct = true;
        character.movement.movePoints_cur = character.movement.movePoints_max;
    }
    private void Character_CantAct(Character character)
    {
        character.canAct = false;
        character.movement.movePoints_cur = 0;
    }
    #endregion

    #region GameEnd
    public IEnumerator Check_Dead(Character character)
    {
        List<BattlePlayer> tempBpList = new List<BattlePlayer>();
        for(int x = 0; x < battlePlayers_List.Count; x++)
        {
            BattlePlayer bp = battlePlayers_List[x];
            if(gameType == Utility.GameType.pvp) if(bp.aiPlayer) continue;

            tempBpList.Add(bp);
        }

        for(int x = 0; x < tempBpList.Count; x++)
        {
            BattlePlayer bp = tempBpList[x];
            if(character == bp.heroCharacter)
            {
                tempBpList.Remove(bp);
                break;
            }
        }

        if(tempBpList.Count != 1) yield break;

        BattlePlayer winner = tempBpList[0];

        string rewardsList = Get_RewardList();
        gameMain.Order_WinLose(winner, rewardsList);
        yield return null;
    }

    public IEnumerator EndGame(BattlePlayer winner, string rewardsList)
    {
        GetComponent<SceneMain_UI>().winLosePanel.Show(winner, rewardsList);
        Heal_AllCharacters();

        if(battlePlayer != winner) yield break;

        ReturnCharacters(winner);
        Give_Reward(winner, rewardsList);

        yield return null;
    }

    private void Heal_AllCharacters()
    {
        for(int x = 0; x < battlePlayers_List.Count; x++)
        {
            BattlePlayer bp = battlePlayers_List[x];
            if(bp.aiPlayer) return;

            // Heal hero
            if(bp.heroCharacter != null)
            {
                Character character = bp.heroCharacter;
                character.health.hp_cur = character.health.hp_max;
            }

            // Heal other
            for(int y = 0; y < bp.availableCharacters.Count; y++)
            {
                Character character = bp.availableCharacters[y];
                character.health.hp_cur = character.health.hp_max;
            }

            for(int y = 0; y < bp.ingameCharacters.Count; y++)
            {
                Character character = bp.ingameCharacters[y];
                character.health.hp_cur = character.health.hp_max;
            }
        }
    }

    private void ReturnCharacters(BattlePlayer bp)
    {
        if(bp.aiPlayer) return;
        List<Character> tempCharList = new List<Character>();

        for(int y = 0; y < bp.availableCharacters.Count; y++)
        {
            Character character = bp.availableCharacters[y];
            tempCharList.Add(character);
        }

        for(int y = 0; y < bp.ingameCharacters.Count; y++)
        {
            Character character = bp.ingameCharacters[y];
            tempCharList.Add(character);
        }

        Account account = GameData.inst.account;
        for(int y = 0; y < tempCharList.Count; y++)
        {
            Character character = tempCharList[y];
            account.heroes[account.battleHeroId].battleCharacters.Add(character);
        }

        LocalData localData = new LocalData();
        localData.Save_PlayerData(account);
    }

    private void Give_Reward(BattlePlayer bp, string rewardsList)
    {
        if(bp.aiPlayer) return;
        Account account = GameData.inst.account;

        if(rewardsList == "") return;

        PlayerItemsData playerItemsData = new PlayerItemsData();
        string[] rewardsData = rewardsList.Split(',');
        for(int x = 0; x < rewardsData.Length; x++)
        {
            PlayerItem playerItem = playerItemsData.Get_PlayerItem_ById(Convert.ToInt32(rewardsData[x]));
            account.items.Add(playerItem);
        }
    }
    #endregion

    private void Setup_Reward()
    {
        int randRewardCount = UnityEngine.Random.Range(1, 10);
        for(int x = 0; x < randRewardCount; x++)
        {
            int rarityValue = UnityEngine.Random.Range(1, 101);
            if(rarityValue <= 5)
            {
                int rareDropValue = UnityEngine.Random.Range(3, 6);
                switch (rareDropValue)
                {
                    case 3:
                        rewards.Add(new Token_Castle());
                    break;
                    case 4:
                        rewards.Add(new Token_Forest());
                    break;
                    case 5:
                        rewards.Add(new Token_Dark());
                    break;
                }
            }
            else if(rarityValue > 5 && rarityValue <= 30)
            {
                rewards.Add(new PiSpearman());
            }
            else if(rarityValue > 30 && rarityValue <= 100)
            {
                rewards.Add(new Gold());
            }
        }
    }

    private string Get_RewardList()
    {
        string rewardsList = "";

        for(int x = 0; x < rewards.Count; x++)
        {
            rewardsList += rewards[x].id + ",";
        }
        if(rewardsList != "") rewardsList = rewardsList.Remove(rewardsList.Length - 1);
        
        return rewardsList;
    }

    public Utility.GameType Get_GameType()
    {
        gameType = GameData.inst.gameType;

        Server server = GameData.inst.server;
        if(server == null) return gameType;

        if(server.players.Count == 1) 
            gameType = Utility.GameType.solo;

        return gameType;
    }

    public void Setup_HeroCharacters()
    {
        for(int x = 0; x < battlePlayers_List.Count; x++)
        {
            if (battlePlayers_List[x].aiPlayer) continue;

            Hex startPoint = startPoints[UnityEngine.Random.Range(0,startPoints.Count)];
            startPoints.Remove(startPoint);

            BattlePlayer bp = battlePlayers_List[x];

            gameMain.Order_CreateHeroCharacter(startPoint, bp);
        }
    }

    private void Setup_Ai()
    {
        if(gameType == Utility.GameType.solo)
        {
            aiBehaviour = new AiSolo(this, battlePlayers_List[battlePlayers_List.Count - 1]);
        }
        else if(gameType == Utility.GameType.pvp)
        {
            aiBehaviour = new AiPvp(this, battlePlayers_List[battlePlayers_List.Count - 1]);
        }
    }
}
