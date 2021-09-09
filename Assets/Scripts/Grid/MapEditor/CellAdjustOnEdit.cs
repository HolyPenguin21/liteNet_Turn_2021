using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode] // COMMENT
// Disable before PlayMode and on Build
public class CellAdjustOnEdit : MonoBehaviour
{
    private GridBuilder gridBuilder;
    private Hex hex;

    void Start()
    {
        hex = GetComponent<Hex>();
        gridBuilder = GameObject.Find("GridBuilder").GetComponent<GridBuilder>();
    }

    void Update()
    {
        transform.position = Get_ClosestGridPos(hex);
    }

    public Vector3 Get_ClosestGridPos(Hex hex)
    {
        float curDist = 10000f;
        Vector3 closestPos = Vector3.zero;

        for (int x = 0; x < gridBuilder.temp_GridCoord.Length; x++)
        {
            Utility.GridCoord gc = gridBuilder.temp_GridCoord[x];

            float dist = Vector3.Distance(hex.transform.position, gc.wPos);
            if (dist < curDist)
            {
                curDist = dist;
                closestPos = gc.wPos;
            }
        }

        return closestPos;
    }
}
