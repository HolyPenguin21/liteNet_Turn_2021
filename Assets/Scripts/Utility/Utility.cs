using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static int enemyHexValue = 3; // move cost near enemy character

    public enum Daytime { dawn, day1, day2, evening, night1, night2 };
    public enum UI_Char_Button { hero, battleChar, accChar };
    public enum GameType { solo, pvp };

    public struct GridCoord
    {
        public int coord_x { get; set;}
        public int coord_y { get; set;}
        public Vector3 wPos { get; set;}
        public int rendValue { get; set;}

        public GridCoord(int x, int y, Vector3 pos, int rendValue)
        {
            this.coord_x = x;
            this.coord_y = y;
            this.wPos = pos;
            this.rendValue = rendValue;
        }
    }

    public static Account Get_Client_Player_ByName(string pName)
    {
        for(int x = 0 ; x < GameData.inst.client.players.Count; x ++)
        {
            if(GameData.inst.client.players[x].name == pName)
                return GameData.inst.client.players[x];
        }

        return null;
    }
    public static int Get_Client_PlayerID_ByName(string pName)
    {
        for(int x = 0 ; x < GameData.inst.client.players.Count; x ++)
        {
            if(GameData.inst.client.players[x].name == pName)
                return x;
        }

        return 0;
    }

    public static Account Get_Server_Player_ByName(string pName)
    {
        for(int x = 0 ; x < GameData.inst.server.players.Count; x ++)
        {
            if(GameData.inst.server.players[x].name == pName)
                return GameData.inst.server.players[x];
        }

        return null;
    }
    public static int Get_Server_PlayerID_ByName(string pName)
    {
        for(int x = 0 ; x < GameData.inst.server.players.Count; x ++)
        {
            if(GameData.inst.server.players[x].name == pName)
                return x;
        }

        return 0;
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

    public static BattlePlayer Get_BattlePlayer_ByName(string name)
    {
        SceneMain sm = GameObject.Find("SceneMain").GetComponent<SceneMain>();

        for(int x = 0; x < sm.bPlayers.Count; x++)
        {
            BattlePlayer bp = sm.bPlayers[x];
            if (bp.name == name)
                return bp;
        }

        return null;
    }

    public static Hex Get_Hex_ByCoords(int coord_x, int coord_y)
    {
        SceneMain sm = GameObject.Find("SceneMain").GetComponent<SceneMain>();

        for(int x = 0; x < sm.grid.Length; x++)
        {
            Hex h = sm.grid[x];

            if(h.coord_x == coord_x && h.coord_y == coord_y)
                return h;
        }

        return null;
    }

    public static bool EnemyInNeighbors(Character character, Hex current)
    {
        for (int x = 0; x < current.neighbors.Count; x++)
        {
            if (current.neighbors[x].character != null && current.neighbors[x].character.tr.gameObject.activeInHierarchy && character.owner != current.neighbors[x].character.owner)
                return true;
        }
        return false;
    }

    public static List<T> Swap_ListItems<T>(List<T> initialList)
    {
        int listHalf = Convert.ToInt32(initialList.Count / 2);

        for (int x = 0; x < listHalf; x++)
        {
            T tempItem = initialList[x];
            initialList[x] = initialList[initialList.Count - 1 - x];
            initialList[initialList.Count - 1 - x] = tempItem;
        }

        return initialList;
    }

    public static int RandomValueGenerator()
    {
        string value = "";
        for(int x = 0 ; x < 9; x++)
        {
            value += UnityEngine.Random.Range(0, 9);
        }

        return Convert.ToInt32(value);
        }
}
