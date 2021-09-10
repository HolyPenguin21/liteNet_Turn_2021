using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    // Editor vars
    public GameObject go;
    public Transform tr;

    // Ingame vars
    public Hex cHex;
    public BattlePlayer owner;
    public Hero cHero; // if character is hero, this should not be NULL

    // Character specific vars
    public int cId;
    public int cCost;
    public Sprite cImage;
    public string cName;
}
