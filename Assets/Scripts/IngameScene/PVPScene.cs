using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPScene
{
    private SceneMain sm;
    private Server server;

    public PVPScene(SceneMain sm)
    {
        this.sm = sm;
        this.server = GameData.inst.server;
    }

    public IEnumerator Setup_Players()
    {
        for(int x = 0; x < server.players.Count; x++)
        {
            Account acc = server.players[x];
            sm.bPlayers.Add(new BattlePlayer(acc, false));
        }

        yield return null;
    }

    public IEnumerator Setup_Characters()
    {
        Debug.Log(sm.bPlayers.Count);
        for(int x = 0; x < sm.bPlayers.Count; x++)
        {
            Hex startPoint = sm.startPoints[Random.Range(0,sm.startPoints.Count)];
            sm.startPoints.Remove(startPoint);

            BattlePlayer bp = sm.bPlayers[x];
            int characterId = bp.hero.character.cId;
            
            GameData.inst.gameMain.Order_CreateCharacter(startPoint, characterId, bp);
        }

        yield return null;
    }
}
