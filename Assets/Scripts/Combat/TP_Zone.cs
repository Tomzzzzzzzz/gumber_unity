using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class TP_Zone : MonoBehaviour
{
    public TP_Zone instance;

    public string nextZone;
    public string actualZone;
    private bool col;
    PlayerController playerController;
    void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && col)
        {
            GameManager.instance.previousZone = actualZone;
            playerController.disabled = true;
            SceneManager.LoadScene(nextZone);
            if (playerController.disabled == true)
            {
                Debug.Log("PlayerController frozen");
            }
        }
    }
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<NetworkIdentity>().netId == NetworkClient.localPlayer.netId)
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            col = true;
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<NetworkIdentity>().netId == NetworkClient.localPlayer.netId)
        {
            col = false;
        }
    }
}
