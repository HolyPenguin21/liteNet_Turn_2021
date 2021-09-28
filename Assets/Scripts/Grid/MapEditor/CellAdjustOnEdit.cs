using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [ExecuteInEditMode] // COMMENT
// Disable before PlayMode and on Build
public class CellAdjustOnEdit : MonoBehaviour
{
    private Transform tr;
    private Hex hex;
    private GridBuilder gridBuilder;

    private Transform[] allChildren;

    void Start()
    {
        tr = transform;
        hex = GetComponent<Hex>();
        gridBuilder = GameObject.Find("GridBuilder").GetComponent<GridBuilder>();
        
        allChildren = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        Utility.GridCoord closestGridCoord = Get_ClosestGridCoord(hex);
        tr.position = closestGridCoord.wPos;
        Adjust_Visual(closestGridCoord);
    }

    private Utility.GridCoord Get_ClosestGridCoord(Hex hex)
    {
        Utility.GridCoord closestGridCoord = gridBuilder.temp_GridCoord[0];
        float curDist = 10000f;

        for (int x = 0; x < gridBuilder.temp_GridCoord.Length; x++)
        {
            Utility.GridCoord gc = gridBuilder.temp_GridCoord[x];

            float dist = Vector3.Distance(hex.transform.position, gc.wPos);
            if (dist < curDist)
            {
                curDist = dist;
                closestGridCoord = gc;
            }
        }

        return closestGridCoord;
    }

    private void Adjust_Visual(Utility.GridCoord gridCoord)
    {
        foreach(Transform child in allChildren)
        {
            SpriteRenderer childRend = child.GetComponent<SpriteRenderer>();
            if(childRend == null) continue;
            
            childRend.sortingOrder = gridCoord.rendValue;
        }
    }
}
