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
    [SerializeField]
    float moveSpeed;
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
        rb = GetComponent<Rigidbody2D>();

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
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        float y = Input.GetAxisRaw("Vertical") * moveSpeed * Time.fixedDeltaTime;

        rb.velocity = new Vector2(x,y);
        rb.velocity.Normalize();
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
