using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public SceneMain sceneMain;
    public SceneMain_UI sceneMain_ui;
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

    #region Block character actions
    public void Order_BlockCharacter(Hex hex)
    {
        BlockCharacter blockCharacter = new BlockCharacter();
        blockCharacter.taskId = Utility.RandomValueGenerator();
        blockCharacter.AssignToAll();

        blockCharacter.coord_x = hex.coord_x;
        blockCharacter.coord_y = hex.coord_y;

        taskManager.AddTask(blockCharacter);
    }
    #endregion

    #region Die
    public void Order_Die(Hex hex)
    {
        DieOrder dieOrder = new DieOrder();
        dieOrder.taskId = Utility.RandomValueGenerator();
        dieOrder.AssignToAll();

        dieOrder.coord_x = hex.coord_x;
        dieOrder.coord_y = hex.coord_y;

        taskManager.AddTask(dieOrder);
    }
    #endregion

    #region Attack
    // Checks before Attack Panel
    public void On_Attack(Hex aHex, Hex tHex)
    {
        Character attacker = aHex.character;
        Character target = tHex.character;

        List<Hex> attackPath = new List<Hex>(sceneMain_ui.pathfinding.Get_Path(aHex, tHex));
        attackPath.RemoveAt(attackPath.Count - 1);

        if(attackPath.Count > 0)
        {
            List<Hex> realPath = sceneMain_ui.pathfinding.Get_RealPath(attacker, attackPath);
            if(realPath == null || attackPath[attackPath.Count - 1] != realPath[realPath.Count - 1])
                return;
        }

        GameObject.Find("SceneMain").GetComponent<SceneMain_UI>().attackPanel.Show(attacker, target);
    }

    public void Request_Attack(Hex aHex, int a_AttackId, Hex tHex, int t_AttackId)
    {
        AttackRequest attackRequest = new AttackRequest();
        
        attackRequest.attacker_x = aHex.coord_x;
        attackRequest.attacker_y = aHex.coord_y;
        attackRequest.a_AttackId = a_AttackId;
        attackRequest.target_x = tHex.coord_x;
        attackRequest.target_y = tHex.coord_y;
        attackRequest.t_AttackId = t_AttackId;

        attackRequest.RequestServer();
    }

    public void Order_Attack(Hex aHex, int a_AttackId, Hex tHex, int t_AttackId)
    {
        Character a_Character = aHex.character;
        Character t_Character = tHex.character;

        List<Hex> attackPath = new List<Hex>(sceneMain_ui.pathfinding.Get_Path(aHex, tHex));
        attackPath.RemoveAt(attackPath.Count - 1);

        Hex attackHex = aHex;

        if (attackPath.Count > 0)
        {
            attackHex = attackPath[attackPath.Count - 1];
            On_Move(aHex, attackHex);
        }

        Order_BlockCharacter(attackHex);

        AttackOrder attackOrder = new AttackOrder();
        attackOrder.taskId = Utility.RandomValueGenerator();
        attackOrder.AssignToAll();

        attackOrder.attacker_x = attackHex.coord_x;
        attackOrder.attacker_y = attackHex.coord_y;
        attackOrder.a_AttackId = a_AttackId;
        attackOrder.target_x = tHex.coord_x;
        attackOrder.target_y = tHex.coord_y;
        attackOrder.t_AttackId = t_AttackId;
        attackOrder.attackData = attackOrder.Get_AttackData(a_Character, t_Character);
        Debug.Log(attackOrder.attackData);
        taskManager.AddTask(attackOrder);
    }
    #endregion

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
    public void On_Move(Hex from, Hex to)
    {
        List<Hex> generalPath = sceneMain_ui.pathfinding.Get_Path(from, to);
        if(generalPath == null || generalPath.Count == 0) return;

        List<Hex> realPath = sceneMain_ui.pathfinding.Get_RealPath(from.character, generalPath);
        if(realPath == null || realPath.Count == 0) return;
        
        int mpLeft = from.character.movement.movePoints_cur - sceneMain_ui.pathfinding.Get_PathCost(from.character, realPath);

        string somePath = "";
        for(int x = 0; x < realPath.Count; x++) {
            Hex hex = realPath[x];
            somePath += hex.coord_x + "," + hex.coord_y + ";";
        }
        if(somePath != "") somePath = somePath.Remove(somePath.Length - 1);

        Order_Move(from, to, mpLeft, somePath);
    }
    
    public void Order_Move(Hex from, Hex to, int mpLeft, string path)
    {
        MoveOrder moveOrder = new MoveOrder();
        moveOrder.taskId = Utility.RandomValueGenerator();
        moveOrder.AssignToAll();

        moveOrder.start_x = from.coord_x;
        moveOrder.start_y = from.coord_y;
        moveOrder.end_x = to.coord_x;
        moveOrder.end_y = to.coord_y;
        moveOrder.mpLeft = mpLeft;
        moveOrder.path = path;

        taskManager.AddTask(moveOrder);
    }

    public void Request_Move(Hex from, Hex to)
    {
        MoveRequest moveRequest = new MoveRequest();
        moveRequest.start_x = from.coord_x;
        moveRequest.start_y = from.coord_y;
        moveRequest.end_x = to.coord_x;
        moveRequest.end_y = to.coord_y;

        moveRequest.RequestServer();
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
