using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public TaskManager taskManager;

#region Ingame
    public void Order_CreateCharacter(Hex hex, int cId, BattlePlayer owner)
    {

        CreateCharacter cr_character = new CreateCharacter();
        cr_character.taskId = Utility.RandomValueGenerator();
        cr_character.AssignToAll();

        cr_character.coord_x = hex.coord_x;
        cr_character.coord_y = hex.coord_y;
        cr_character.characterId = cId;
        cr_character.ownerName = owner.name;

        taskManager.AddTask(cr_character);
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
