using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TP_Zone : MonoBehaviour
{
    public string nextZone;
    public string actualZone;
    public bool col;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && col)
            SceneManager.LoadScene(nextZone);
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
