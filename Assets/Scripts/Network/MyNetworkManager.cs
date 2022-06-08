using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using kcp2k;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Mirror;
public class MyNetworkManager : NetworkManager
{
    public GameObject Assassin;
    public GameObject Support;
    public GameObject Paladin;
    public GameObject Mage;
    public GameObject Hunter;


    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server started!");
        NetworkServer.RegisterHandler<CreateMMOCharacterMessage>(CustomInstantiate);
    }
    public override void OnStopServer()
    {
        Debug.Log("Server stopped!");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Connected to server!");
        CreateMMOCharacterMessage characterMessage = new CreateMMOCharacterMessage
        {
            race = CharacterManager.race,
            pseudo = CharacterManager.pseudo
        };
        Debug.Log("Avant message");
        //CustomInstantiate((NetworkConnectionToClient)NetworkClient.connection, characterMessage);
        NetworkClient.Send(characterMessage);
        Debug.Log("Après message");
    }

    public override void OnClientDisconnect()
    {
        Debug.Log("Disconnected from server!");
    }

    
    void CustomInstantiate(NetworkConnectionToClient conn, CreateMMOCharacterMessage message)
    {
        Debug.Log("Appel sur CustomInstantiate");
        (string pseudo, string race) = (message.pseudo, message.race);
        switch(race)
        {
            case "Paladin":
            {
                Debug.Log("Création paladin");
                GameObject Player = Instantiate(Paladin);
                PlayerData data = Player.GetComponent<PlayerData>();
                data.pseudo = pseudo;
                data.race = race;
                NetworkServer.AddPlayerForConnection(conn, Player);
                break;
            }
            
            case "Support":
            {
                GameObject Player = Instantiate(Support);
                PlayerData data = Player.GetComponent<PlayerData>();
                data.pseudo = pseudo;
                data.race = race;
                NetworkServer.AddPlayerForConnection(conn, Player);
                break;
            }
            case "Hunter":
            {
                GameObject Player = Instantiate(Hunter);
                PlayerData data = Player.GetComponent<PlayerData>();
                data.pseudo = pseudo;
                data.race = race;
                NetworkServer.AddPlayerForConnection(conn, Player);
                break;
            }
            case "Mage":
            {
                GameObject Player = Instantiate(Mage);
                PlayerData data = Player.GetComponent<PlayerData>();
                data.pseudo = pseudo;
                data.race = race;
                NetworkServer.AddPlayerForConnection(conn, Player);
                break;
            }
            case "Assassin":
            {
                GameObject Player = Instantiate(Assassin);
                PlayerData data = Player.GetComponent<PlayerData>();
                data.pseudo = pseudo;
                data.race = race;
                NetworkServer.AddPlayerForConnection(conn, Player);
                break;
            }
            default:
            {
                break;
            }

        }
    }

}