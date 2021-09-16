using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LiteNetLib;
using LiteNetLib.Utils;

public class Server : MonoBehaviour
{
    // Initial
    private EventBasedNetListener netListener;
    public NetManager netManager;
    public NetPacketProcessor netProcessor;
    private ServerSubscriptions serverSubscriptions;

    public List<Account> players = new List<Account>();

    public TaskManager taskManager;
    public List<GeneralNetworkTask> taskList = new List<GeneralNetworkTask>();
    public GeneralNetworkTask taskCurrent;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        taskManager = new TaskManager(this);
        GameObject.Find("GameMain").GetComponent<GameMain>().taskManager = taskManager; // bad

        netListener = new EventBasedNetListener();
        netManager = new NetManager(netListener);
        netProcessor = new NetPacketProcessor();

        netListener.ConnectionRequestEvent += request =>
        {
            request.AcceptIfKey("v0.0.1");
        };

        netListener.PeerConnectedEvent += client =>
        {
            Debug.Log("Server > someone is connected, asking to login..." + client.EndPoint);

            Account somePlayer = new Account("Name_NotLoggedIn");
            somePlayer.address = client;
            players.Add(somePlayer);

            LoginRequest loginRequest = new LoginRequest();
            netProcessor.Send(client, loginRequest, DeliveryMethod.ReliableOrdered);

            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                MenuSceneMain menuSceneMain = GameObject.Find("SceneMain").GetComponent<MenuSceneMain>();
                menuSceneMain.UpdateConnectedPlayersList();
            }
        };

        netListener.PeerDisconnectedEvent += (client, info) =>
        {
            Debug.Log("Server > someone is disconnected");

            Account disconnectedPlayer = null;
            for (int x = 0; x < players.Count; x++)
            {
                if (players[x].address == client)
                    disconnectedPlayer = players[x];
            }
            players.Remove(disconnectedPlayer);

            serverSubscriptions.Update_PlayersListOnClients();

            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                MenuSceneMain menuSceneMain = GameObject.Find("SceneMain").GetComponent<MenuSceneMain>();
                menuSceneMain.UpdateConnectedPlayersList();
                menuSceneMain.Remove_PlayerPanel(disconnectedPlayer);
            }
        };

        netListener.NetworkReceiveEvent += (someClient, dataReader, deliveryMethod) =>
        {
            netProcessor.ReadAllPackets(dataReader, someClient);
        };

        netManager.Start(10515);

        Account serverPlayer = GameData.inst.account;
        serverPlayer.isServer = true;
        players.Add(serverPlayer);

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            MenuSceneMain menuSceneMain = GameObject.Find("SceneMain").GetComponent<MenuSceneMain>();
            menuSceneMain.UpdateConnectedPlayersList();
        }

        serverSubscriptions = new ServerSubscriptions(this, netProcessor);
        // Custom methods
        serverSubscriptions.LoginResponse();
        serverSubscriptions.HeroChange();
        serverSubscriptions.TaskDone();
        serverSubscriptions.SetTurn();
    }

    private void Update()
    {
        netManager.PollEvents();
        taskManager.CheckTask();
    }

    public void StopServer()
    {
        netManager.Stop();
        netListener.ClearConnectionRequestEvent();
		netListener.ClearPeerConnectedEvent();
        Destroy(gameObject);
    }
}
