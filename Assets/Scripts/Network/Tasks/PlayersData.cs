using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersData : GeneralNetworkTask
{
    public string players { get; set; }
    public string pHeroes { get; set; }
    public string pCharacters { get; set; }
    public string pItems { get; set; }

    // Data to be send to all Clients to update them
    public override IEnumerator Implementation_Server()
    {
        Server server = GameData.inst.server;

        this.players = Get_All_PlayersData(server);
        this.pHeroes = Get_All_PlayersHeroesData(server);
        this.pCharacters = Get_All_PlayersCharactersData(server);
        this.pItems = Get_All_PlayersItemsData(server);

        // Debug.Log("Server : Players : " + this.players); // pName,bhId|pName,bhId
        // Debug.Log("Server : Heroes data : " + this.pHeroes); // 0,hName,hcId:1,1,1;1,hName,chId:111|
        // Debug.Log("Server : Player characters : " + this.pCharacters);

        yield return null;
    }

    private string Get_All_PlayersData(Server s)
    {
        string data = "";

        for(int x = 0; x < s.players.Count; x++)
        {
            Account acc = s.players[x];
            data += acc.name + "," + acc.battleHeroId + "|";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_All_PlayersHeroesData(Server s)
    {
        string data = "";

        for(int x = 0; x < s.players.Count; x++)
        {
            Account p = s.players[x];

            data += Get_PlayerHeroes(x, p);
            data += "|";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_PlayerHeroes(int ownerId, Account p)
    {
        string data = "";

        for(int x = 0; x < p.heroes.Count; x++)
        {
            Hero h = p.heroes[x];
            data += ownerId + "," + h.name + "," + h.character.cId + ":";
            data += Get_HeroCharacters(h);
            data += ";";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_HeroCharacters(Hero h)
    {
        string data = "";

        for(int x = 0; x < h.battleCharacters.Count; x++)
        {
            Character c = h.battleCharacters[x];
            data += c.cId + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_All_PlayersCharactersData(Server s)
    {
        string data = "";

        for(int x = 0; x < s.players.Count; x++)
        {
            Account p = s.players[x];

            data += x + ":";
            data += Get_PlayerCharacters(p);
            data += "|";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_PlayerCharacters(Account p)
    {
        string data = "";

        for(int x = 0; x < p.сharacters.Count; x++)
        {
            Character c = p.сharacters[x];
            data += c.cId + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_All_PlayersItemsData(Server s)
    {
        string data = "";

        for(int x = 0; x < s.players.Count; x++)
        {
            Account p = s.players[x];

            data += x + ":";
            data += Get_PlayerItems(p);
            data += "|";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }

    private string Get_PlayerItems(Account p)
    {
        string data = "";

        for(int x = 0; x < p.items.Count; x++)
        {
            PlayerItem pi = p.items[x];
            data += pi.id + ",";
        }
        if(data != "") data = data.Remove(data.Length - 1);

        return data;
    }


    public override IEnumerator Implementation_Client()
    {
        // Debug.Log("Client : Players : " + this.players); // pName,pName
        // Debug.Log("Client : Heroes data : " + this.pHeroes); // 0,hName,hcId:1,1,1;1,hName,chId:111|
        // Debug.Log("Client : Player characters : " + this.pCharacters);

        Client client = GameData.inst.client;

        Set_Players(client);            // pName,bhId|pName,bhId
        Set_PlayersHeroes(client);      // 0,h_zxc_1,1:1;0,h_zxc_2,2:1|1,h_qwe_1,1:1;1,h_qwe_2,2:1
        Set_PlayersCharacters(client);  // 0:1|1:1
        Set_PlayersItems(client);       // 0:1|1:1

        // for(int x = 0; x < client.players.Count; x++)
        // {
        //     string pInfo = "";
        //     Player p = client.players[x];
        //     pInfo += "pName : " + p.pName + " ";
        //     for(int y = 0; y < p.pHeroes.Count; y++)
        //     {
        //         Hero h = p.pHeroes[y];
        //         pInfo += "hName : " + h.hName + " sCharCount : " + h.selectedCharacters.Count + " ";
        //     }
        //     pInfo += "pCharsCount : " + p.pCharacters.Count;
        //     Debug.Log(pInfo);
        // }

        yield return null;
    }

    private void Set_Players(Client c)
    {
        // pName,bhId|pName,bhId
        c.players.Clear();

        string[] data = this.players.Split('|');
        for(int x = 0; x < data.Length; x++)
        {
            string[] acc_data = data[x].Split(',');

            string acc_name = acc_data[0];
            int acc_battleHeroId = Convert.ToInt32(acc_data[1]);

            Account acc = new Account(acc_name);
            acc.battleHeroId = acc_battleHeroId;

            c.players.Add(acc);
        }
    }

    private void Set_PlayersHeroes(Client c)
    {
        // 0,h_zxc_1,1:1;0,h_zxc_2,2:1|1,h_qwe_1,1:1;1,h_qwe_2,2:1
        string[] data = this.pHeroes.Split('|');
        for(int x = 0; x < data.Length; x++)
        {
            string[] hero = data[x].Split(';');
            for(int y = 0; y < hero.Length; y++)
            {
                string[] hero_char_selected = hero[y].Split(':');
                string[] hero_char = hero_char_selected[0].Split(','); // Hero

                Character hCharacter = Utility.Get_Character_ById(Convert.ToInt32(hero_char[2]));
                Hero h = new Hero(hCharacter, hero_char[1]);

                string[] hero_selected = hero_char_selected[1].Split(','); // Selected
                for(int z = 0; z < hero_selected.Length; z++)
                {
                    Character selectedCharacter = Utility.Get_Character_ById(Convert.ToInt32(hero_selected[z]));
                    h.battleCharacters.Add(selectedCharacter);
                }

                c.players[Convert.ToInt32(hero_char[0])].heroes.Add(h);
            }
        }
    }

    private void Set_PlayersCharacters(Client c)
    {
        // 0:1,1|1:1,1
        string[] data = this.pCharacters.Split('|');
        for(int x = 0; x < data.Length; x++)
        {
            string[] acc_data = data[x].Split(':');

            int accId = Convert.ToInt32(acc_data[0]);
            Account acc = c.players[accId];

            if(acc_data[1] == "") break;

            string[] char_data = acc_data[1].Split(',');
            for(int y = 0; y < char_data.Length; y++)
            {
                int charId = Convert.ToInt32(char_data[y]);
                Character character = Utility.Get_Character_ById(charId);
                acc.сharacters.Add(character);
            }
        }
    }

    private void Set_PlayersItems(Client c)
    {
        // 0:1,1|1:1,1
        string[] data = this.pItems.Split('|');
        for(int x = 0; x < data.Length; x++)
        {
            string[] acc_data = data[x].Split(':');

            int accId = Convert.ToInt32(acc_data[0]);
            Account acc = c.players[accId];

            if(acc_data[1] == "") break;

            string[] char_data = acc_data[1].Split(',');
            for(int y = 0; y < char_data.Length; y++)
            {
                int itemId = Convert.ToInt32(char_data[y]);
                PlayerItem pi = Utility.Get_PlayerItem_ById(itemId);
                acc.items.Add(pi);
            }
        }
    }

    public override void SendToClients(Server server)
    {
    }

    public override void RequestServer()
    {
    }
}
