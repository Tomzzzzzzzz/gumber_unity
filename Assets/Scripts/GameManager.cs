using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<PlayerData> closePlayers;

    private bool col;
    
    [HideInInspector]
    public string previousZone;
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

    // Start is called before the first frame update
    void Start()
    {
        InfoPlayerManager.instance.panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
