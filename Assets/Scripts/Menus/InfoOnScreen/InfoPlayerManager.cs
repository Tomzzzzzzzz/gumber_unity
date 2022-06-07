using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class InfoPlayerManager : MonoBehaviour
{
    public Text hp, level;
    public GameObject panel;
    public static InfoPlayerManager instance;

    private PlayerController player; 
    private void Awake()
    {
        instance = this;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
        {
            GetPlayer();
        }
        else
        {
            level.text = (player.xp / 100).ToString();
            hp.text = player.currentHealth.ToString();
        }
    }

    void GetPlayer()
    {
        player = NetworkClient.localPlayer.gameObject.GetComponent<PlayerController>();
    }
}
