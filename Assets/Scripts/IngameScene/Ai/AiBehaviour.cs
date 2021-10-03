using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiBehaviour
{
    public SceneMain sceneMain;
    public SceneMain_UI sceneMain_ui;
    
    public BattlePlayer aiBattlePlayer;
    public bool aiInAction;

    public abstract IEnumerator AITurn();
}
