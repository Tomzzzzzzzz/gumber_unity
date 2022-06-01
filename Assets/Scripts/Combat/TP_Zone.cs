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
    public bool col;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && col)
        {
            GameManager.instance.previousZone = actualZone;
            SceneManager.LoadScene(nextZone);
        }
    }
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            col = true;
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            col = false;
        }
    }
}
