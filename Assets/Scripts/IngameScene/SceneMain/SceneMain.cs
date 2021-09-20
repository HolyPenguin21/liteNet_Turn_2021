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
    public List<BattlePlayer> bPlayers = new List<BattlePlayer>();
    public BattlePlayer currentTurn;

    public Utility.GameType gameType;
    public List<PlayerItem> rewards = new List<PlayerItem>();

    // UI
    public Text currentTurn_Text;
    public Button endTurn_Button;

    private void Awake()
    {
        // UI
        currentTurn_Text = GameObject.Find("CurrentTurn_Text").GetComponent<Text>();
        endTurn_Button = GameObject.Find("EndTurn_Button").GetComponent<Button>();
    }

    private IEnumerator Start()
    {
        gm = GameData.inst.gameMain;
        yield return Setup_Game();

        yield return null;
    }

    private IEnumerator Setup_Game()
    {
        gameType = GameData.inst.gameType;

        // UI
        endTurn_Button.interactable = false;

        if(gameType == Utility.GameType.solo)
        {

        }
        else if(gameType == Utility.GameType.pvp)
        {
            yield return Setup_Players();
            yield return Setup_Characters();
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
            bpId = Random.Range(0, bPlayers.Count);
        }

        gm.Order_SetTurn(bpId);

        yield return null;
    }

    public void Button_EndTurn()
    {
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;

        if(server != null) {
            int bpId = bPlayers.IndexOf(currentTurn);
            bpId++;
            if(bpId >= bPlayers.Count) bpId = 0;
            gm.Order_SetTurn(bpId);
        }
        else {
            gm.Request_EndTurn();
        }
    }

    // Will be called on both Host and Client
    public void On_TurnChange()
    {
        currentTurn_Text.text = "Current turn for : " + currentTurn.name;

        if(GameData.inst.server != null && currentTurn.aiPlayer) Button_EndTurn(); // put AI logic here

        if(myBPlayer == currentTurn) endTurn_Button.interactable = true;
        else endTurn_Button.interactable = false;

        for(int x = 0; x < bPlayers.Count; x++) 
        {
            BattlePlayer bp = bPlayers[x];
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
    }
    #endregion

    #region GameStart
    public IEnumerator Setup_Players()
    {
        Server server = GameData.inst.server;
        Client client = GameData.inst.client;
        
        if(server != null) {
            for(int x = 0; x < server.players.Count; x++)
            {
                Account acc = server.players[x];
                BattlePlayer bp = new BattlePlayer(acc, false);
                bPlayers.Add(bp);
                if(bp.name == GameData.inst.account.name) myBPlayer = bp;
            }
        }
        else {
            for(int x = 0; x < client.players.Count; x++)
            {
                Account acc = client.players[x];
                BattlePlayer bp = new BattlePlayer(acc, false);
                bPlayers.Add(bp);
                if(bp.name == GameData.inst.account.name) myBPlayer = bp;
            }
        }

        BattlePlayer bpAI = new BattlePlayer(null, true);
        bPlayers.Add(bpAI);

        yield return null;
    }

    public IEnumerator Setup_Characters()
    {
        Server server = GameData.inst.server;
        if(server == null) yield break;

        for(int x = 0; x < bPlayers.Count; x++) {
            if (bPlayers[x].aiPlayer) continue;

            Hex startPoint = startPoints[Random.Range(0,startPoints.Count)];
            startPoints.Remove(startPoint);

            BattlePlayer bp = bPlayers[x];
            int characterId = bp.hero.character.id;
            
            gm.Order_CreateCharacter(startPoint, characterId, bp);
        }

        yield return null;
    }
    #endregion
}
