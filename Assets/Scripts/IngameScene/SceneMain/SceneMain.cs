using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMain : MonoBehaviour
{
    private GameMain gameMain;
    public Hex[] grid;
    public List<Hex> startPoints = new List<Hex>();
    public List<Hex> bossSpawners = new List<Hex>();

    public BattlePlayer battlePlayer;
    public List<BattlePlayer> battlePlayers_List = new List<BattlePlayer>();
    public BattlePlayer currentTurn;

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

        gameMain = GameData.inst.gameMain;
        gameMain.sceneMain = this;

        Utility.Set_InputType();
    }

    private IEnumerator Start()
    {
        Server server = GameData.inst.server;
        if(server == null) yield break;

        yield return new WaitForSeconds(2f);
        Setup_Reward();
        gameMain.Order_SetupPvpPlayers();



        // Setup_Players();
        // yield return Setup_Game();
    }



    private IEnumerator Setup_Game()
    {
        Get_GameType();
        yield return Setup_HeroCharacters();

        endTurn_Button.interactable = false;

        if(gameType == Utility.GameType.solo)
        {
            aiBehaviour = new AiSolo(this, battlePlayers_List[battlePlayers_List.Count - 1]);
        }
        else if(gameType == Utility.GameType.pvp)
        {
            aiBehaviour = new AiPvp(this, battlePlayers_List[battlePlayers_List.Count - 1]);
        }

        yield return Setup_FirstTurn();
    }

    #region Turn management
    public IEnumerator Setup_FirstTurn()
    {
        Server server = GameData.inst.server;
        if(server == null) yield break;

        int bpId = 0;
        if(gameType == Utility.GameType.solo)
        {
            
        }
        else if(gameType == Utility.GameType.pvp)
        {
            bpId = UnityEngine.Random.Range(0, battlePlayers_List.Count);
        }

        gameMain.Order_SetTurn(bpId);

        yield return null;
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
            for(int y = 0; y < bp.ingameCharacters.Count; y++) 
            {
                Character character = bp.ingameCharacters[y];
                if(bp == currentTurn) 
                {
                    character.canAct = true;
                    character.movement.movePoints_cur = character.movement.movePoints_max;
                }
                else 
                {
                    character.canAct = false;
                    character.movement.movePoints_cur = 0;
                }
            }
        }

        if(GameData.inst.server != null && currentTurn.aiPlayer) 
            StartCoroutine(aiBehaviour.AITurn());

        yield return null;
    }
    #endregion

    #region GameStart
    public Utility.GameType Get_GameType()
    {
        gameType = GameData.inst.gameType;

        Server server = GameData.inst.server;
        if(server == null) return gameType;

        if(server.players.Count == 1) 
            gameType = Utility.GameType.solo;

        return gameType;
    }

    public IEnumerator Setup_HeroCharacters()
    {
        Server server = GameData.inst.server;
        if(server == null) yield break;

        for(int x = 0; x < battlePlayers_List.Count; x++)
        {
            if (battlePlayers_List[x].aiPlayer) continue;

            Hex startPoint = startPoints[UnityEngine.Random.Range(0,startPoints.Count)];
            startPoints.Remove(startPoint);

            BattlePlayer bp = battlePlayers_List[x];

            gameMain.Order_CreateHeroCharacter(startPoint, bp);
        }

        yield return null;
    }
    #endregion

    #region GameEnd
    public IEnumerator Check_Dead(Character character)
    {
        // Server only
        BattlePlayer winner = null;

        List<BattlePlayer> tempBpList = new List<BattlePlayer>();
        for(int x = 0; x < battlePlayers_List.Count; x++)
        {
            BattlePlayer bp = battlePlayers_List[x];

            if(gameType == Utility.GameType.pvp)
                if(bp.aiPlayer) continue;

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

        winner = tempBpList[0];

        string rewardsList = "";
        for(int x = 0; x < rewards.Count; x++)
        {
            rewardsList += rewards[x].id + ",";
        }
        if(rewardsList != "") rewardsList = rewardsList.Remove(rewardsList.Length - 1);

        gameMain.Order_WinLose(winner, rewardsList);
        yield return null;
    }

    public IEnumerator EndGame(BattlePlayer winner, string rewardsList)
    {
        GetComponent<SceneMain_UI>().winLosePanel.Show(winner, rewardsList);

        for(int x = 0; x < battlePlayers_List.Count; x++)
        {
            BattlePlayer someBattlePlayer = battlePlayers_List[x];
            if(someBattlePlayer.heroCharacter != null)
            {
                Character character = someBattlePlayer.heroCharacter;
                character.health.hp_cur = character.health.hp_max;
            }

            if(someBattlePlayer != battlePlayer || someBattlePlayer != winner) continue;

            yield return ReturnCharacters(someBattlePlayer);
            yield return Give_Reward(rewardsList);
        }
    }

    private IEnumerator ReturnCharacters(BattlePlayer battlePlayer)
    {
        List<Character> tempCharList = new List<Character>();

        for(int y = 0; y < battlePlayer.availableCharacters.Count; y++)
        {
            Character character = battlePlayer.availableCharacters[y];
            tempCharList.Add(character);
        }

        for(int y = 0; y < battlePlayer.ingameCharacters.Count; y++)
        {
            Character character = battlePlayer.ingameCharacters[y];
            if(character == battlePlayer.heroCharacter) continue;
            
            tempCharList.Add(character);
        }

        Account account = GameData.inst.account;
        for(int y = 0; y < tempCharList.Count; y++)
        {
            Character character = tempCharList[y];
            character.health.hp_cur = character.health.hp_max;

            account.heroes[account.battleHeroId].battleCharacters.Add(character);
        }

        LocalData localData = new LocalData();
        localData.Save_PlayerData(account);
        yield return null;
    }

    private IEnumerator Give_Reward(string rewardsList)
    {
        Account account = GameData.inst.account;

        PlayerItemsData playerItemsData = new PlayerItemsData();
        string[] rewardsData = rewardsList.Split(',');
        for(int x = 0; x < rewardsData.Length; x++)
        {
            PlayerItem playerItem = playerItemsData.Get_PlayerItem_ById(Convert.ToInt32(rewardsData[x]));
            account.items.Add(playerItem);
        }

        yield return null;
    }
    #endregion

    private void Setup_Reward()
    {
        int randRewardCount = UnityEngine.Random.Range(1, 10);
        for(int x = 0; x < randRewardCount; x++)
        {
            int chanceOfDrop = UnityEngine.Random.Range(1, 101);
            if(chanceOfDrop <= 50) continue;

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
            else if(rarityValue > 5 && rarityValue <= 50)
            {
                rewards.Add(new Gold());
            }
            else if(rarityValue > 50 && rarityValue <= 100)
            {
                rewards.Add(new Gold());
            }
        }
    }
}
