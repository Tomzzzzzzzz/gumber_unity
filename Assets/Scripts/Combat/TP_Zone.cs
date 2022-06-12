using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class TP_Zone : NetworkBehaviour
{
    public TP_Zone instance;

    public string nextZone;
    public string actualZone;
    private bool col;

    void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && col)
        {
            GameManager.instance.previousZone = actualZone;
            NetworkClient.localPlayer.gameObject.GetComponent<PlayerController>().enabled = false;
            SceneManager.LoadScene(nextZone);
        }
    }
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (isServer) {return;}
        if (collision.tag == "Player" && collision.gameObject.GetComponent<NetworkIdentity>().netId == NetworkClient.localPlayer.netId)
        {
            col = true;
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (isServer) {return;}
        if (collision.tag == "Player" && collision.gameObject.GetComponent<NetworkIdentity>().netId == NetworkClient.localPlayer.netId)
        {
            col = false;
        }
    }
}
