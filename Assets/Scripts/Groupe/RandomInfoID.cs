using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class RandomInfoID : MonoBehaviour
{
    public int id;

    public void Invite()
    {
        Debug.Log("Inviting...");
        NetworkIdentity identity = GameManager.instance.closePlayers[id].GetComponentInParent<NetworkIdentity>();
        string pseudo = NetworkClient.localPlayer.GetComponent<PlayerData>().pseudo;
        GroupManager group = GameObject.Find("GroupCanvas").GetComponent<GroupManager>();
        NetworkClient.localPlayer.GetComponent<PlayerData>().Invite(identity, pseudo, group.playersInGroup);
    }
}
