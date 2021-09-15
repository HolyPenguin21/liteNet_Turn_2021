using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LiteNetLib;
using LiteNetLib.Utils;

public class ClientSubscriptions
{
    private Client client;
    private NetPacketProcessor netProcessor;

    public ClientSubscriptions(Client client, NetPacketProcessor netProcessor)
    {
        this.client = client;
        this.netProcessor = netProcessor;
    }

    public void LoginRequest()
    {
        netProcessor.SubscribeReusable<LoginRequest>((data) => {
            Debug.Log("Client > Login request from Server.");

            LoginResponse loginResponse = new LoginResponse();
            GameData.inst.StartCoroutine(loginResponse.Implementation_Client());

            netProcessor.Send(client.netManager.GetPeerById(0), loginResponse, DeliveryMethod.ReliableOrdered);
        });
    }

    public void PlayersData()
    {
        netProcessor.SubscribeReusable<PlayersData>((data) => {
            Debug.Log("Client > Players list recieved.");
            PlayersData playersData = data;
            GameData.inst.StartCoroutine(playersData.Implementation_Client());

            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                MenuSceneMain menuSceneMain = GameObject.Find("SceneMain").GetComponent<MenuSceneMain>();
                menuSceneMain.UpdateConnectedPlayersList();
                menuSceneMain.Client_OpenLobby_OnConnect();
                
                for (int x = 0; x < client.players.Count; x++)
                {
                    menuSceneMain.Add_PlayerPanel(client.players[x]);
                }
            }
        });
    }

    public void HeroChange()
    {
        netProcessor.SubscribeReusable<HeroChange>((data) => {
            // Debug.Log("Client > HeroChange order recieved.");
            HeroChange heroChange = data;
            GameData.inst.StartCoroutine(heroChange.Implementation_Client());
        });
    }

    public void CreateCharacter()
    {
        netProcessor.SubscribeReusable<CreateCharacter>((data) => {
            Debug.Log("Client > CreateCharacter order from Server. Task id : " + data.taskId);
            CreateCharacter charCreation = data;
            GameData.inst.StartCoroutine(charCreation.Implementation_Client());
        });
    }

    public void SetTurn()
    {
        netProcessor.SubscribeReusable<SetTurn>((data) => {
            Debug.Log("Client > SetTurn order from Server. Task id : " + data.taskId);
            SetTurn setTurn = data;
            GameData.inst.StartCoroutine(setTurn.Implementation_Client());
        });
    }

    public void MoveOrder()
    {
        netProcessor.SubscribeReusable<Move>((data) => {
            Debug.Log("Client > Move order from Server. Task id : " + data.taskId);
            Move move = data;
            GameData.inst.StartCoroutine(move.Implementation_Client());
        });
    }

    public void PVPGameStart()
    {
        netProcessor.SubscribeReusable<PVPGameStart>((data) => {
            Debug.Log("Client > GameStart order from Server. Task id : " + data.taskId);
            PVPGameStart pvpStart = data;
            GameData.inst.StartCoroutine(pvpStart.Implementation_Client());
        });
    }
}
