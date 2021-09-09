using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiteNetLib;

public class Account
{
    public NetPeer address;

    public bool isServer = false;
    public string name;
    public string password;

    public List<Hero> heroes;
    public List<Character> сharacters;
    public List<PlayerItem> items;

    public int acc_gold;

    public int battleHeroId = 0;

    public Account (string name)
    {
        this.name = name;

        this.heroes = new List<Hero>();
        this.сharacters = new List<Character>();
        this.items = new List<PlayerItem>();
    }
}
