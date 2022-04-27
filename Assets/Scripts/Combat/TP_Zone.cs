using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TP_Zone : MonoBehaviour
{
    public TP_Zone instance;

    public string nextZone;
    public string actualZone;
    public bool col;

    void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (GameManager.instance == null) Debug.Log("NUUUUUUUUUUUUUUUUUUUUUUUUL");
        if (Input.GetKeyDown(KeyCode.E) && col)
        {
            Debug.Log("trying to load map...");
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
