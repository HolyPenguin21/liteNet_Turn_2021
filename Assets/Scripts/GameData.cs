using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static GameData inst;
    public GameMain gameMain;

    public Account account;

    public Server server;
    public Client client;

    public Utility.GameType gameType;

    private void Awake()
    {
        gameMain = GetComponent<GameMain>();

        DontDestroyOnLoad(gameObject);
        inst = this;
    }

    public void CreateHost()
    {
        GameObject serverObj = Instantiate(Resources.Load("Network/Server", typeof(GameObject))) as GameObject;
        server = serverObj.GetComponent<Server>();
    }

    public void CreateClient(string ip)
    {
        GameObject clientObj = Instantiate(Resources.Load("Network/Client", typeof(GameObject))) as GameObject;
        client = clientObj.GetComponent<Client>();
        client.Connect(ip);
    }

    public void Close_ServerClient()
    {
        if(server != null)
        {
            server.StopServer();
            server = null;
        }

        if(client != null)
        {
            client.StopClient();
            client = null;
        }

        Debug.Log("Sys > Server and Client are removed.");
    }

    private void OnApplicationQuit()
    {
        Close_ServerClient();
    }
}
