using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerData : NetworkBehaviour
{
    public string pseudo;
    public string race;

    public GameObject prefab;
    public uint id;
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
            Debug.Log("Start PlayerData");
            pseudo = CharacterManager.pseudo;
            id = NetworkClient.localPlayer.netId;
            race = CharacterManager.race;
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

}
