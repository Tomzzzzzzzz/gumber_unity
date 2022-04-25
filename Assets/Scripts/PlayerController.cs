using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;
    string pseudo;
    //Animator anim;

    [Header("Stat")]
    public int maxHealth;
    public int currentHealth;
    public static int money;
    public /*static ?*/ int xp;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
