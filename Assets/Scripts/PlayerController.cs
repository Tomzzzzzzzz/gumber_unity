using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerController : NetworkBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;
    string pseudo;
    Animator anim;

    [Header("Stat")]
    [SerializeField]
    float moveSpeed;
    public int maxHealth;
    public int currentHealth;
    public static int money;
    public /*static ?*/ int xp;

    private float litmitSpeed = 0.7f;

    public static PlayerController instance;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }
        UpdateCharacter(selectedOption);

        
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer && this != null)
        {
            Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject.transform);
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (isLocalPlayer && this != null)
        {
            if (Input.GetAxis("Horizontal") > 0.1 || Input.GetAxis("Horizontal") < -0.1 ||
                Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical") < -0.1)
            {
                anim.SetFloat("LastInputX", Input.GetAxis("Horizontal"));
                anim.SetFloat("LastInputY", Input.GetAxis("Vertical"));
            }

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(x,y) * moveSpeed * Time.fixedDeltaTime;
            rb.velocity.Normalize();
            if (x != 0 || y != 0)
            {
                anim.SetFloat("InputX",x);
                anim.SetFloat("InputY",y);
            }

        }
        
    }

    //
    // LOAD SKIN 
    //
    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
    private int selectedOption = 0;
    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }

}
