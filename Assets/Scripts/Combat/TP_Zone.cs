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
        if (Input.GetKeyDown(KeyCode.E) && col)
        {
            SceneManager.LoadScene(nextZone);
            CombatManager.instance.PreviousScene = actualZone;
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
