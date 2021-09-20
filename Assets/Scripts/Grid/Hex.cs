using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public Transform tr;
    
    public int rendValue;
    public int coord_x;
    public int coord_y;
    public List<Hex> neighbors = new List<Hex>();

    public bool groundMove;
    public int moveCost = 1;

    public Hex rootCastle;      // set in editor
    public bool isStartPoint;   // set in editor
    public bool isBossSpawner;  // set in editor

    public Character character;

    private void Awake()
    {
        tr = transform;
        VisualAdjust();
    }

    public void Set_Editor_Name()
    {
        string result = coord_x + "," + coord_y;

        if(isStartPoint) result += " -StartPoint";
        if(isBossSpawner) result += " -BossSpawner";
        if(rootCastle != null) result += " -SummonPoint";

        gameObject.name = result;
    }

    private void VisualAdjust()
    {
        rendValue = tr.Find("background").GetComponent<SpriteRenderer>().sortingOrder;
    }
}
