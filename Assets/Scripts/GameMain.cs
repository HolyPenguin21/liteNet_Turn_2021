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

    #region Turn management
    public void Order_SetTurn(int bpId)
    {
        SetTurn setTurn = new SetTurn();
        setTurn.taskId = Utility.RandomValueGenerator();
        setTurn.AssignToAll();
        
        setTurn.bpId = bpId;

        taskManager.AddTask(setTurn);
    }

    public void Request_EndTurn()
    {
        SetTurn setTurn = new SetTurn();
        setTurn.RequestServer();
    }
    #endregion

    #region Movement
    public void On_Move(Pathfinding pathfinding, Hex from, Hex to)
    {
        List<Hex> generalPath = pathfinding.Get_Path(from, to);
        if(generalPath == null || generalPath.Count == 0) return;

        List<Hex> realPath = pathfinding.Get_RealPath(from.character, generalPath);
        if(realPath == null || realPath.Count == 0) return;
        
        int mpLeft = from.character.movement.movePoints_cur - pathfinding.Get_PathCost(from.character, realPath);

        string somePath = "";
        for(int x = 0; x < realPath.Count; x++) {
            Hex h = realPath[x];
            somePath += h.coord_x + "," + h.coord_y + ";";
        }
        if(somePath != "") somePath = somePath.Remove(somePath.Length - 1);

        Order_Move(from, to, mpLeft, somePath);
    }
    
    public void Order_Move(Hex from, Hex to, int mpLeft, string path)
    {
        Move move = new Move();
        move.taskId = Utility.RandomValueGenerator();
        move.AssignToAll();

        move.start_x = from.coord_x;
        move.start_y = from.coord_y;
        move.end_x = to.coord_x;
        move.end_y = to.coord_y;
        move.mpLeft = mpLeft;
        move.path = path;

        taskManager.AddTask(move);
    }

    public void Request_Move(Hex from, Hex to)
    {
        RequestMove requestMove = new RequestMove();
        requestMove.start_x = from.coord_x;
        requestMove.start_y = from.coord_y;
        requestMove.end_x = to.coord_x;
        requestMove.end_y = to.coord_y;

        requestMove.RequestServer();
    }
    #endregion
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
