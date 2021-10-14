using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public LineRenderer pathVisual;
    public List<Hex> path;
    private List<Hex> temp_path;
    private List<Hex> visual_path;

    public Pathfinding(LineRenderer pathVisual)
    {
        this.pathVisual = pathVisual;
        path = new List<Hex>();
        temp_path = new List<Hex>();
        visual_path = new List<Hex>();
    }

    public void Hide_Path()
    {
        pathVisual.gameObject.SetActive(false);
    }

    public void Show_Path(Hex start, Hex end)
    {
        temp_path = Get_Path(start, end);
        if(temp_path == null || temp_path.Count == 0) return;
        
        visual_path.Clear();
        visual_path.Add(start);
        for(int x = 0; x < temp_path.Count; x++)
            visual_path.Add(temp_path[x]);

        pathVisual.positionCount = visual_path.Count;
        for (int x = 0; x < pathVisual.positionCount; x++)
        {
            pathVisual.SetPosition(x, visual_path[x].tr.position);
        }

        pathVisual.gameObject.SetActive(true);
    }

    public void Show_RealPath(Character character, Hex start, Hex end)
    {
        temp_path = Get_RealPath(character, Get_Path(start, end));
        if(temp_path == null || temp_path.Count == 0) return;
        
        visual_path.Clear();
        visual_path.Add(start);
        for(int x = 0; x < temp_path.Count; x++)
            visual_path.Add(temp_path[x]);

        pathVisual.positionCount = visual_path.Count;
        for (int x = 0; x < pathVisual.positionCount; x++)
        {
            pathVisual.SetPosition(x, visual_path[x].tr.position);
        }

        pathVisual.gameObject.SetActive(true);
    }

    public List<Hex> Get_Path(Hex start, Hex end)
    {
        if(end == null) return null;

        bool pathComplete = false;
        List<Hex> finalPath = new List<Hex>();

        Queue<Hex> groupToVisit = new Queue<Hex>();
        groupToVisit.Enqueue(start);

        Dictionary<Hex, int> costSoFar = new Dictionary<Hex, int>();
        costSoFar[start] = 0;

        Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>();
        cameFrom[start] = start;

        while (groupToVisit.Count > 0)
        {
            Hex current = groupToVisit.Dequeue();

            foreach (Hex next in current.neighbors)
            {
                if (next != end && next.character != null) continue;

                int newCost = costSoFar[current] + next.moveCost;
                if (Utility.EnemyInNeighbors(start.character, current) && Utility.EnemyInNeighbors(start.character, next))
                    newCost += Utility.enemyHexValue;

                if (next.groundMove && (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]))
                {
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                    groupToVisit.Enqueue(next);
                }
            }
        }

        if (cameFrom.ContainsKey(end)) pathComplete = true;
        if (!pathComplete) return null;

        finalPath.Add(end);

        Hex rebuildPoint = cameFrom[end];
        while (rebuildPoint != start)
        {
            finalPath.Add(rebuildPoint);

            rebuildPoint = cameFrom[rebuildPoint];
        }

        finalPath = Utility.Swap_ListItems(finalPath);
        
        path.Clear();
        path = finalPath;
        
        return finalPath;
    }

    public List<Hex> Get_RealPath(Character character, List<Hex> somePath)
    {
        if (somePath == null) return null;

        List<Hex> realPath = new List<Hex>();

        int movePointsLeft = character.movement.movePoints_cur;
        Hex current = character.hex;
        Hex next = somePath[0];

        for (int x = 0; x < somePath.Count; x++)
        {
            movePointsLeft -= somePath[x].moveCost;
            if (Utility.EnemyInNeighbors(character, current) && Utility.EnemyInNeighbors(character, next))
                movePointsLeft -= Utility.enemyHexValue;

            if (movePointsLeft >= 0)
                realPath.Add(somePath[x]);

            if (x != somePath.Count - 1)
            {
                current = somePath[x];
                next = somePath[x + 1];
            }
        }

        if (realPath.Count == 0)
            return null;
        else
            return realPath;
    }

    public int Get_PathCost(Character character, List<Hex> path)
    {
        if (path == null) return 0;

        int cost = 0;
        Hex current = character.hex;
        Hex next = path[0];

        for (int x = 0; x < path.Count; x++)
        {
            cost += path[x].moveCost;
            if (Utility.EnemyInNeighbors(character, current) && Utility.EnemyInNeighbors(character, next))
                cost += Utility.enemyHexValue;

            if (x != path.Count - 1)
            {
                current = path[x];
                next = path[x + 1];
            }
        }

        return cost;
    }
}
