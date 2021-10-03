using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LiteNetLib;
using LiteNetLib.Utils;

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

    #region Get Client Player
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
    #endregion

    #region Get Server Player
    public static Account Get_Server_Player_ByAddress(NetPeer client)
    {
        Server server = GameData.inst.server;
        for(int x = 0 ; x < server.players.Count; x ++)
        {
            Account acc = server.players[x];
            if(acc.address == client)
                return acc;
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
    public static Account Get_Server_Player_ByName(string pName)
    {
        for(int x = 0 ; x < GameData.inst.server.players.Count; x ++)
        {
            if(GameData.inst.server.players[x].name == pName)
                return GameData.inst.server.players[x];
        }

        return null;
    }
    #endregion

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

        for(int x = 0; x < sm.battlePlayers.Count; x++)
        {
            BattlePlayer battlePlayer = sm.battlePlayers[x];
            if (battlePlayer.name == name)
                return battlePlayer;
        }

        return null;
    }

    public static Hex Get_Hex_ByCoords(int coord_x, int coord_y) 
    {
        SceneMain sm = GameObject.Find("SceneMain").GetComponent<SceneMain>();

        for(int x = 0; x < sm.grid.Length; x++) {
            Hex hex = sm.grid[x];
            if(hex.coord_x == coord_x && hex.coord_y == coord_y)
                return hex;
        }

        return null;
    }

    public static Hex Get_Hex_ByTransform(Transform tr) 
    {
        SceneMain sm = GameObject.Find("SceneMain").GetComponent<SceneMain>();

        for(int x = 0 ; x < sm.grid.Length; x++) {
            Hex hex = sm.grid[x];
            if(hex.tr == tr) return hex;
        }

        return null;
    }

    public static List<Hex> Get_HexPath_ByCoords(string coords)
    {
        List<Hex> path = new List<Hex>();

        string[] pathData = coords.Split(';');
        for (int x = 0; x < pathData.Length; x++)
        {
            string[] hexCoords = pathData[x].Split(',');
            int posX = int.Parse(hexCoords[0]);
            int posY = int.Parse(hexCoords[1]);

            path.Add(Get_Hex_ByCoords(posX, posY));
        }

        return path;
    }

    public static bool IsMyCharacter(Character character)
    {
        SceneMain sm = GameObject.Find("SceneMain").GetComponent<SceneMain>();
        if(character.owner == sm.myBPlayer) return true;
        return false;
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

    public static void Set_InputType()
    {
        // Text tooltipInput = GameObject.Find("Tooltip_Input_Text").GetComponent<Text>();
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            GameData.inst.inputPc = false;
            GameData.inst.panCamera = true;
            // tooltipInput.text = "Input : Android";
            Debug.Log("PC input is set");
        }
        else
        {
            GameData.inst.inputPc = true;
            GameData.inst.panCamera = false;
            // tooltipInput.text = "Input : PC";
            Debug.Log("Android input is set");
        }
    }

    public static void Load_Scene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
