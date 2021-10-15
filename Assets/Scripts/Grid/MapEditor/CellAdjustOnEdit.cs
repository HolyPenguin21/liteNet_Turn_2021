using UnityEngine;
using UnityEditor; // COMMENT

[ExecuteInEditMode] // COMMENT
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
            if( childRend == null) continue;
            
            if (child.name == "background")
            {
                childRend.sortingOrder = gridCoord.rendValue;
            }
            else if (child.name == "front")
            {
                childRend.sortingOrder = gridCoord.rendValue + 1;
            }
            else
            {
                childRend.sortingOrder = gridCoord.rendValue - 1;
            }
        }
    }
}
