using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPvp : AiBehaviour
{
    public AiPvp(SceneMain sceneMain, BattlePlayer aiBattlePlayer)
    {
        this.sceneMain = sceneMain;
        this.aiBattlePlayer = aiBattlePlayer;
    }

    public override IEnumerator AITurn()
    {
        sceneMain.Button_EndTurn();
        yield return null;
    }
}
