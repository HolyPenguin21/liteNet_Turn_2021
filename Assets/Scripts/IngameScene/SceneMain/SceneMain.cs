using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMain : MonoBehaviour
{
    private GameMain gm;
    public Hex[] grid;
    public List<Hex> startPoints = new List<Hex>();
    public List<Hex> bossSpawners = new List<Hex>();

    public BattlePlayer myBPlayer;
    public List<BattlePlayer> battlePlayers = new List<BattlePlayer>();
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

        gm = GameData.inst.gameMain;
        gm.sceneMain = this;
    }

    private IEnumerator Start()
    {
        Utility.Set_InputType();

        yield return Setup_Game();

        yield return null;
    }

    private IEnumerator Setup_Game()
    {
        Get_GameType();
        yield return Setup_Reward();
        yield return Setup_Players();
        yield return Setup_HeroCharacters();

        endTurn_Button.interactable = false;

        if(gameType == Utility.GameType.solo)
        {
            aiBehaviour = new AiSolo(this, battlePlayers[battlePlayers.Count - 1]);
        }
        else if(gameType == Utility.GameType.pvp)
        {
            aiBehaviour = new AiPvp(this, battlePlayers[battlePlayers.Count - 1]);
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
            bpId = Random.Range(0, battlePlayers.Count);
        }

        gm.Order_SetTurn(bpId);

        yield return null;
    }

    public void Button_EndTurn()
    {
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;

        if(server != null) {
            int bpId = battlePlayers.IndexOf(currentTurn);
            bpId++;
            if(bpId >= battlePlayers.Count) bpId = 0;
            gm.Order_SetTurn(bpId);
        }
        else {
            gm.Request_EndTurn();
        }
    }

    // Will be called on both Host and Client
    public IEnumerator On_TurnChange()
    {
        currentTurn_Text.text = "Current turn for : " + currentTurn.name;

        if(myBPlayer == currentTurn) endTurn_Button.interactable = true;
        else endTurn_Button.interactable = false;

        for(int x = 0; x < battlePlayers.Count; x++) 
        {
            BattlePlayer bp = battlePlayers[x];
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

    public IEnumerator Setup_Reward()
    {
        Server server = GameData.inst.server;
        if(server == null) yield break;

        rewards.Add(new Gold());
        rewards.Add(new Gold());
        yield return null;
    }

    public IEnumerator Setup_Players()
    {
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;
        Account account = null;
        BattlePlayer bp = null;
        LocalData localData = new LocalData();
        
        if(server != null) 
        {
            for(int x = 0; x < server.players.Count; x++)
            {
                account = server.players[x];
                bp = new BattlePlayer(this, account, false);
                battlePlayers.Add(bp);
                if(bp.name == GameData.inst.account.name)
                {
                    myBPlayer = bp;
                    localData.Save_PlayerData(account);
                }
            }
        }
        else 
        {
            for(int x = 0; x < client.players.Count; x++)
            {
                account = client.players[x];
                bp = new BattlePlayer(this, account, false);
                battlePlayers.Add(bp);
                if(bp.name == GameData.inst.account.name)
                {
                    myBPlayer = bp;
                    localData.Save_PlayerData(account);
                }
            }
        }

        BattlePlayer aiBattlePlayer = new BattlePlayer(this, null, true);
        battlePlayers.Add(aiBattlePlayer);

        yield return null;
    }

    public IEnumerator Setup_HeroCharacters()
    {
        Server server = GameData.inst.server;
        if(server == null) yield break;

        for(int x = 0; x < battlePlayers.Count; x++)
        {
            if (battlePlayers[x].aiPlayer) continue;

            Hex startPoint = startPoints[Random.Range(0,startPoints.Count)];
            startPoints.Remove(startPoint);

            BattlePlayer bp = battlePlayers[x];

            gm.Order_CreateHeroCharacter(startPoint, bp);
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
        for(int x = 0; x < battlePlayers.Count; x++)
        {
            BattlePlayer bp = battlePlayers[x];

            if(gameType == Utility.GameType.pvp)
                if(bp.aiPlayer) continue;

            tempBpList.Add(bp);
        }

        for(int x = 0; x < tempBpList.Count; x++)
        {
            BattlePlayer bp = tempBpList[x];
            if(character == bp.hero.character)
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

        gm.Order_WinLose(winner, rewardsList);
        yield return null;
    }

    public IEnumerator EndGame(BattlePlayer winner, string rewardsList)
    {
        GetComponent<SceneMain_UI>().winLosePanel.Show(winner, rewardsList);

        for(int x = 0; x < battlePlayers.Count; x++)
        {
            BattlePlayer bp = battlePlayers[x];
            if(bp != myBPlayer || bp != winner) continue;

            yield return ReturnCharacters(bp);
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
            if(character == battlePlayer.hero.character) continue;
            
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
        yield return null;
    }
    #endregion
}
