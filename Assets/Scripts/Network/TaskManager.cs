using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager
{
    private Server server;

    public TaskManager(Server server)
    {
        this.server = server;
    }

    public void CheckTask()
    {
        if(server.taskCurrent != null && server.taskCurrent.assignedPlayers.Count == 0)
        {
            Debug.Log("Task is done by all players : " + server.taskCurrent + " " + server.taskCurrent.taskId);
            server.taskCurrent = null;
        }

        if(server.taskCurrent == null && server.taskList.Count > 0)
        {
            server.taskCurrent = server.taskList[0];
            server.taskList.RemoveAt(0);

            server.StartCoroutine(server.taskCurrent.Implementation_Server());
        }
    }

    public void AddTask(GeneralNetworkTask task)
    {
        server.taskList.Add(task);
    }
}
