using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static float distHexes = 1.3f;

    public enum Daytime { dawn, day1, day2, evening, night1, night2 };
    public enum GameType { solo, pvp };

    public struct GridCoord
    {
        public int coord_x { get; set;}
        public int coord_y { get; set;}
        public Vector3 wPos { get; set;}

        public GridCoord(int x, int y, Vector3 pos)
        {
            coord_x = x;
            coord_y = y;
            wPos = pos;
        }
    }

    public static Account Get_Client_Player_ByName(string pName)
    {
        Account p = null;

        for(int x = 0 ; x < GameData.inst.client.players.Count; x ++)
        {
            if(GameData.inst.client.players[x].name == pName)
                p = GameData.inst.client.players[x];
        }

        return p;
    }
    public static int Get_Client_PlayerID_ByName(string pName)
    {
        int result = 0;

        for(int x = 0 ; x < GameData.inst.client.players.Count; x ++)
        {
            if(GameData.inst.client.players[x].name == pName)
                result = x;
        }

        return result;
    }

    public static Account Get_Server_Player_ByName(string pName)
    {
        Account p = null;

        for(int x = 0 ; x < GameData.inst.server.players.Count; x ++)
        {
            if(GameData.inst.server.players[x].name == pName)
                p = GameData.inst.server.players[x];
        }

        return p;
    }
    public static int Get_Server_PlayerID_ByName(string pName)
    {
        int result = 0;

        for(int x = 0 ; x < GameData.inst.server.players.Count; x ++)
        {
            if(GameData.inst.server.players[x].name == pName)
                result = x;
        }

        return result;
    }

    public static Character Get_Character_ById(int id)
    {
        switch(id)
        {
            case 1:
                return new Swordman(null, null, null);
            case 2:
                return new Spearman(null, null, null);
            case 3:
                return new Knight(null, null, null);
            default:
                return new Swordman(null, null, null);
        }
    }

    public static Sprite Get_CharacterImage_ById(int id)
    {
        switch(id)
        {
            case 1:
                return Resources.Load<Sprite>("Characters/Swordman/Swordman_ii");
            case 2:
                return Resources.Load<Sprite>("Characters/Spearman/Spearman_ii");
            case 3:
                return Resources.Load<Sprite>("Characters/Knight/Knight_ii");
            default:
                return Resources.Load<Sprite>("Characters/Swordman/Swordman_ii");
        }
    }

    public static PlayerItem Get_PlayerItem_ById(int id)
    {
        switch(id)
        {
            case 1:
                return new TreasureChest();
            default:
                return new TreasureChest();
        }
    }

    public static int RandomValueGenerator()
    {
        string value = "";
        for(int x = 0 ; x < 9; x++)
        {
            value += UnityEngine.Random.Range(0, 9);
        }
        int fValue = Convert.ToInt32(value);
        return fValue;
        }
}
