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

    private PlayerData player;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            level.text = (player.xp / 100).ToString();
            hp.text = $"{player.currentHealth}/{player.maxHealth}";
        }
    }

    public void GetPlayer()
    {
        player = NetworkClient.localPlayer.gameObject.GetComponent<PlayerData>();
    }
}
