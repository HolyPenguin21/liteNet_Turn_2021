using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloScene
{
    private SceneMain sm;

    public SoloScene(SceneMain sm)
    {
        this.sm = sm;
    }

    public IEnumerator Setup_Players()
    {
        sm.bPlayers.Add(new BattlePlayer(GameData.inst.account, false));
        sm.bPlayers.Add(new BattlePlayer(new Account("AI"), true));

        yield return null;
    }

    public IEnumerator Setup_Characters()
    {
        Hex startPoint = sm.startPoints[Random.Range(0,sm.startPoints.Count)];
        int characterId = sm.bPlayers[0].hero.character.cId;
        BattlePlayer bp = sm.bPlayers[0];

        GameData.inst.gameMain.Order_CreateCharacter(startPoint, characterId, bp);

        yield return null;
    }

    public IEnumerator Setup_FirstTurn()
    {
        sm.currentTurn = sm.bPlayers[0];

        yield return null;
    }
}
