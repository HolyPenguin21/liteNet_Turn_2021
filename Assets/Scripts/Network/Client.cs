using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LiteNetLib;
using LiteNetLib.Utils;

public class Client : MonoBehaviour
{
    // Initial
    private string loginKey = "v0.0.1";
    private EventBasedNetListener netListener;
    public NetManager netManager;
    public NetPacketProcessor netProcessor;

    private ClientSubscriptions clientSubscriptions;

    public List<Account> players = new List<Account>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Connect(string ip)
    {
        netListener = new EventBasedNetListener();
        netManager = new NetManager(netListener);
        netProcessor = new NetPacketProcessor();

        netListener.NetworkReceiveEvent += (client, dataReader, deliveryMethod) =>
        {
            netProcessor.ReadAllPackets(dataReader, client);
        };

        netListener.ConnectionRequestEvent += request =>
        {
            request.Reject();
        };

        netListener.PeerDisconnectedEvent += (peer, info) =>
        {
            if(netManager.GetPeerById(0) == peer)
                if(SceneManager.GetActiveScene().buildIndex == 0)
                    GameObject.Find("SceneMain").GetComponent<MenuSceneMain>().Button_P2P_Lobby_Back();
        };

        netManager.Start();
        netManager.Connect(ip, 10515, loginKey);

        clientSubscriptions = new ClientSubscriptions(this, netProcessor);
        // Custom methods
        clientSubscriptions.LoginRequest();
        clientSubscriptions.PlayersData();
        clientSubscriptions.MoveOrder();
        clientSubscriptions.HeroChange();
        clientSubscriptions.PVPGameStart();
        clientSubscriptions.CreateCharacter();
        clientSubscriptions.SetTurn();
    }

    private void Update()
    {
        netManager.PollEvents();
    }

    public void StopClient()
    {
        netManager.Stop();
        netListener.ClearConnectionRequestEvent();
		netListener.ClearPeerConnectedEvent();
        Destroy(gameObject);
    }
}
