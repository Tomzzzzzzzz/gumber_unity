using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class InfoPlayerManager : NetworkBehaviour
{
    public Text hp, level;
    public GameObject panel;
    public static InfoPlayerManager instance;

    private PlayerData player;

    private void Awake()
    {
        instance = this;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            level.text = (player.xp / 100).ToString();
            hp.text = player.currentHealth.ToString();
        }
    }

    public void GetPlayer()
    {
        player = NetworkClient.localPlayer.gameObject.GetComponent<PlayerData>();
    }
}
