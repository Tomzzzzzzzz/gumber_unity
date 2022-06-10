using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
public class PlayerData : NetworkBehaviour
{
    [SyncVar]
    public string pseudo;
    [HideInInspector] public string race;

    public GameObject prefab;

    public static PlayerData instance;
    public Sprite headSprite;

    [SyncVar(hook = nameof(OnMaxHealthChange))]
    //[SerializeField]
    public int maxHealth;

    [SyncVar(hook = nameof(OnCurrentHealthChange))]
    //[SerializeField]
    public int currentHealth;

    //[SerializeField]
    [SyncVar(hook = nameof(OnXpChange))]
    public int xp;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            prefab.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            pseudo = CharacterManager.pseudo;
            race = CharacterManager.race;
            GroupManager.instance.playersInGroup[0] = this;
            //transform.Find("InviteCanvas").GetChild()
        }
    }

    void Update()
    {
        if (isLocalPlayer && Input.GetKey(KeyCode.Space))
        {
            prefab.transform.position = NetworkClient.localPlayer.GetComponent<Transform>().position;
            Debug.Log("Try to spawn");
            Test();
        }
    }

    void OnMaxHealthChange(int oldInt, int newInt)
    {
        Debug.Log($"old maxHealth : {oldInt} & new maxHealth : {newInt}");
        maxHealth = newInt;
    }

    void OnCurrentHealthChange(int oldInt, int newInt)
    {
        Debug.Log($"old currentHealth : {oldInt} & new currentHealth : {newInt}");
        currentHealth = newInt;
    }

    void OnXpChange(int oldInt, int newInt)
    {
        Debug.Log($"old xp : {oldInt} & new xp : {newInt}");
        xp = newInt;
    }

    [Command]
    public void AddXp(int nb)
    {
        xp += nb;
    }

    [Command]
    public void SetPseudo(string name)
    {
        pseudo = name;
    }

    [Command]
    public void Test()
    {
        GameObject mob = Instantiate(prefab);
        NetworkServer.Spawn(mob);
    }

    [Command]
    public void Invite(NetworkIdentity identity, string pseudo, PlayerData[] playersInGroup)
    {
        Debug.Log("Inviting Player...");
        ReceiveInvitation(identity.connectionToClient,pseudo, playersInGroup);
    }

    [TargetRpc]
    public void ReceiveInvitation(NetworkConnection target, string pseudo, PlayerData[] playersInGroup)
    {
        Debug.Log("Invitation received");
        GameObject InvitePanel;
        InvitePanel = GameObject.Find("/InviteCanvas/Invitation");
        InvitePanel.transform.position = new Vector3(960,1030,0);
        InvitePanel.transform.Find("Pseudo").GetComponent<TextMeshProUGUI>().text = pseudo;
        GameObject.Find("GroupCanvas").GetComponent<GroupManager>().playersInGroup = playersInGroup;
    }

    [Command]
    public void AcceptInvitation(NetworkIdentity identity, PlayerData newMember)
    {
        Debug.Log("Coucou je suis le serveur et j'invite un joueur");
        AddPlayerToGroup(identity.connectionToClient, newMember);
    }

    [TargetRpc]
    public void AddPlayerToGroup(NetworkConnection target, PlayerData newMember)
    {
        Debug.Log("Récupération du membre invité...");
        int i = 0;
        PlayerData[] group = GameObject.Find("GroupCanvas").GetComponent<GroupManager>().playersInGroup;
        while (i < 4 && group[i] != null)
        {
            Debug.Log($"Le {i}eme joueur dans le groupe est {group[i].pseudo}");
            i++;
        }
        if (i < 4)
        {
            Debug.Log("i < 4");
            group[i] = newMember;
        }
    }
}
