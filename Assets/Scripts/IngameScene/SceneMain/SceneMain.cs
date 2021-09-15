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

    public List<BattlePlayer> bPlayers = new List<BattlePlayer>();
    public BattlePlayer currentTurn;

    public Utility.GameType gameType;
    public List<PlayerItem> rewards = new List<PlayerItem>();


    // UI
    public Text currentTurn_Text;

    private void Awake()
    {
        // UI
        currentTurn_Text = GameObject.Find("CurrentTurn_Text").GetComponent<Text>();
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
                bPlayers.Add(new BattlePlayer(acc, false));
            }
        }
        else {
            for(int x = 0; x < client.players.Count; x++)
            {
                Account acc = client.players[x];
                bPlayers.Add(new BattlePlayer(acc, false));
            }
        }
        yield return null;
    }

    public IEnumerator Setup_Characters()
    {
        Server server = GameData.inst.server;
        if(server == null) yield break;

        for(int x = 0; x < bPlayers.Count; x++) {
            Hex startPoint = startPoints[Random.Range(0,startPoints.Count)];
            startPoints.Remove(startPoint);

            BattlePlayer bp = bPlayers[x];
            int characterId = bp.hero.character.cId;
            
            gm.Order_CreateCharacter(startPoint, characterId, bp);
        }

        yield return null;
    }
    #endregion

    #region Other
    public Hex Get_Hex_ByTransform(Transform tr) {
        for(int x = 0 ; x < grid.Length; x++) {
            Hex h = grid[x];
            if(h.tr == tr) return h;
        }

        return null;
    }
    #endregion
}
