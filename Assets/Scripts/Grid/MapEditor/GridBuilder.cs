using System.Collections.Generic;
using UnityEngine;
// using UnityEditor; // COMMENT

// [ExecuteInEditMode] // COMMENT
public class GridBuilder : MonoBehaviour
{
    public int grid_height = 5;
    public int grid_width = 5;

    private SceneMain sceneData;
    public GameObject tileTemplate_pref;
    private float cell_width = 1.552f;
    private float cell_height = 0.3f;

    public Utility.GridCoord[] temp_GridCoord;
    public List<Utility.GridCoord> temp_GameGrid;

    void Awake()
    {
        temp_GameGrid = new List<Utility.GridCoord>();

        sceneData = GameObject.Find("SceneMain").GetComponent<SceneMain>();

        CreateGrid();
        Assign_HexToGrid();
        Assign_Neighbors();
    }

    private void CreateGrid()
    {
        temp_GridCoord = new Utility.GridCoord[grid_height * grid_width];

        int current = 0;
        for (int x = 0; x < grid_width; x++)
        {
            for (int y = 0; y < grid_height; y++)
            {
                string coords = x + "," + y;

                float posX = (x * cell_width) + (y * cell_width / 2) - (y / 2) * cell_width;
                float posY = y * cell_height;

                // Grid visual
                Vector3 worldPos = new Vector3(posX, posY, 0);
                GameObject hexVisual = Instantiate(tileTemplate_pref, worldPos, Quaternion.identity, transform);
                hexVisual.name = coords;
                hexVisual.transform.Find("Cell_Pos_Text").GetComponent<TextMesh>().text = coords;
                hexVisual.transform.Find("background").GetComponent<SpriteRenderer>().sortingOrder = -y;

                temp_GridCoord[current] = new Utility.GridCoord(x,y,worldPos, -y);

                current++;
            }
        }

        Debug.Log("Scene builder > Grid created. Hex count : " + temp_GridCoord.Length);
    }

    private void Assign_HexToGrid()
    {
        for (int x = 0 ; x < temp_GridCoord.Length; x++)
        {
            Utility.GridCoord gc = temp_GridCoord[x];
            Hex h = Get_ClosestHex(gc.wPos);
            if (h == null) continue;
            temp_GameGrid.Add(gc);
        }

        sceneData.grid = new Hex[temp_GameGrid.Count];
        sceneData.startPoints.Clear();
        sceneData.bossSpawners.Clear();
        for(int x = 0; x < temp_GameGrid.Count; x++)
        {
            Utility.GridCoord gc = temp_GameGrid[x];
            Hex h = Get_ClosestHex(gc.wPos);

            h.coord_x = gc.coord_x;
            h.coord_y = gc.coord_y;
            h.Set_Editor_Name();
            h.tr = h.transform;

            sceneData.grid[x] = h;

            if (h.isStartPoint)
                sceneData.startPoints.Add(h);

            if (h.isBossSpawner)
                sceneData.bossSpawners.Add(h);
        }

        // EditorUtility.SetDirty(sceneData); // COMMENT
        Debug.Log("Scene builder > Hexes assigned to grid.");
    }

    private void Assign_Neighbors()
    {
        for(int x = 0; x < sceneData.grid.Length; x++)
        {
            Hex current = sceneData.grid[x];
            Get_Neighbors(current);

            // EditorUtility.SetDirty(current); // COMMENT
        }

        Debug.Log("Scene builder > Neighbors are setted up");
    }

    private void Get_Neighbors(Hex hex)
    {
        hex.neighbors.Clear();

        for(int x = 0; x < sceneData.grid.Length; x++)
        {
            Hex toCheck = sceneData.grid[x];
            if (hex == toCheck) continue;

            float dist = Vector3.Distance(hex.tr.position, toCheck.tr.position);
            if (dist < 0.85f) 
            hex.neighbors.Add(toCheck);
        }
    }

    private Hex Get_ClosestHex(Vector3 pos)
    {
        GameObject[] hexes = GameObject.FindGameObjectsWithTag("Hex");

        Hex closestHex = null;
        float curDist = 0.75f;

        for (int x = 0; x < hexes.Length; x++)
        {
            float dist = Vector3.Distance(hexes[x].transform.position, pos);
            if (dist < curDist)
            {
                curDist = dist;
                if (dist < 0.5f) closestHex = hexes[x].GetComponent<Hex>();
            }
        }

        return closestHex;
    }
}
