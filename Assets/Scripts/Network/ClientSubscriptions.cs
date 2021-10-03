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

    public void WinLoseOrder()
    {
        netProcessor.SubscribeReusable<WinLoseOrder>((data) => {
            Debug.Log("Client > WinLoseOrder from Server. Task id : " + data.taskId);

            WinLoseOrder winLoseOrder = data;
            GameData.inst.StartCoroutine(winLoseOrder.Implementation_Client());
        });
    }

    public void ChangeGoldOrder()
    {
        netProcessor.SubscribeReusable<ChangeGoldOrder>((data) => {
            Debug.Log("Client > ChangeGoldOrder from Server. Task id : " + data.taskId);

            ChangeGoldOrder changeGoldOrder = data;
            GameData.inst.StartCoroutine(changeGoldOrder.Implementation_Client());
        });
    }

    public void DieOrder()
    {
        netProcessor.SubscribeReusable<DieOrder>((data) => {
            Debug.Log("Client > DieOrder from Server. Task id : " + data.taskId);

            DieOrder dieOrder = data;
            GameData.inst.StartCoroutine(dieOrder.Implementation_Client());
        });
    }

    public void HireOrder()
    {
        netProcessor.SubscribeReusable<HireOrder>((data) => {
            Debug.Log("Client > HireOrder from Server. Task id : " + data.taskId);

            HireOrder hireOrder = data;
            GameData.inst.StartCoroutine(hireOrder.Implementation_Client());
        });
    }

    public void AttackOrder()
    {
        netProcessor.SubscribeReusable<AttackOrder>((data) => {
            Debug.Log("Client > AttackOrder from Server. Task id : " + data.taskId);

            AttackOrder attackOrder = data;
            GameData.inst.StartCoroutine(attackOrder.Implementation_Client());
        });
    }

    public void BlockCharacter()
    {
        netProcessor.SubscribeReusable<BlockCharacter>((data) => {
            Debug.Log("Client > BlockCharacter order from Server.");

            BlockCharacter blockCharacter = data;
            GameData.inst.StartCoroutine(blockCharacter.Implementation_Client());
        });
    }

    public void CreateHeroCharacter()
    {
        netProcessor.SubscribeReusable<CreateHeroCharacter>((data) => {
            Debug.Log("Client > CreateHeroCharacter order from Server. Task id : " + data.taskId);

            CreateHeroCharacter createHeroCharacter = data;
            GameData.inst.StartCoroutine(createHeroCharacter.Implementation_Client());
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

    public void CreateAICharacter()
    {
        netProcessor.SubscribeReusable<CreateAICharacter>((data) => {
            Debug.Log("Client > CreateCharacter order from Server. Task id : " + data.taskId);

            CreateAICharacter createAICharacter = data;
            GameData.inst.StartCoroutine(createAICharacter.Implementation_Client());
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
        netProcessor.SubscribeReusable<MoveOrder>((data) => {
            Debug.Log("Client > Move order from Server. Task id : " + data.taskId);
            MoveOrder moveOrder = data;
            GameData.inst.StartCoroutine(moveOrder.Implementation_Client());
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

    public void PVPGameStart()
    {
        netProcessor.SubscribeReusable<PVPGameStart>((data) => {
            Debug.Log("Client > GameStart order from Server. Task id : " + data.taskId);
            PVPGameStart pvpStart = data;
            GameData.inst.StartCoroutine(pvpStart.Implementation_Client());
        });
    }
}
