using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public TaskManager taskManager;
    private CharactersData cData;

    private void Awake()
    {
        cData = new CharactersData();
    }

#region Ingame
    public void Order_CreateCharacter(Hex hex, int cId, BattlePlayer owner)
    {
        StartCoroutine(cData.Create_Character(hex, cId, owner));
    }

    public void Order_Move()
    {
        Move move = new Move();
        move.taskId = Utility.RandomValueGenerator();
        move.AssignToAll();

        move.start_x = 3;
        move.start_y = 4;
        move.end_x = 5;
        move.end_y = 6;
        move.mpLeft = 2;
        move.path = "1,2;3,4;5,6;7,8";

        taskManager.AddTask(move);
    }
    #endregion

#region Lobby_HeroChange
    public void Order_HeroChange(string pName, int hId)
    {
        HeroChange heroChange = new HeroChange();
        heroChange.taskId = Utility.RandomValueGenerator();
        heroChange.AssignToAll();

        heroChange.pName = pName;
        heroChange.hId = hId;
        
        taskManager.AddTask(heroChange);
    }

    public void Request_HeroChange(string pName, int hId)
    {
        HeroChange heroChange = new HeroChange();
        heroChange.pName = pName;
        heroChange.hId = hId;
        heroChange.RequestServer();
    }
    #endregion

#region Lobby_StartGame
    public void Order_StartGame(int sceneId)
    {
        PVPGameStart pvpStart = new PVPGameStart();
        pvpStart.taskId = Utility.RandomValueGenerator();
        pvpStart.AssignToAll();

        pvpStart.sceneId = sceneId;

        taskManager.AddTask(pvpStart);
    }
    #endregion
}
