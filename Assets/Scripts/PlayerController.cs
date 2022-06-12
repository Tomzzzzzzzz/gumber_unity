using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerController : NetworkBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;
    Animator anim;

    [Header("Stat")]
    [SerializeField]
    float moveSpeed;
    //private float litmitSpeed = 0.7f;
    public static PlayerController instance;
    public CharacterDatabase characterDB;
    public Sprite artworkSprite;
    public Sprite characterHead;
    
    [HideInInspector] public SpriteRenderer Renderer;
    private int selectedOption = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
        if (isLocalPlayer)
        {
            if (!PlayerPrefs.HasKey("selectedOption"))
            {
                selectedOption = 0;
            }
            else
            {
                Load();
            }
            UpdateCharacter(selectedOption);
            /*
            pseudo = CharacterManager.pseudo;
            id = NetworkClient.localPlayer.netId;
            */
            InfoPlayerManager.instance.GetPlayer();
            InfoPlayerManager.instance.panel.SetActive(true);
        }
        else
        {
            Renderer.sprite = artworkSprite;
        }
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
        if (!hasAuthority)
        {
            return;
        }
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
    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite = character.characterSprite;
        characterHead = character.headSprite;
        Renderer.sprite = character.characterSprite;
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
        //pseudo = PlayerPrefs.GetString("pseudo");
        //Debug.Log($"PC pseudo : {pseudo}");
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (isLocalPlayer && collision.tag == "Player" &&
            !IsAlreadyInList(collision.gameObject.GetComponent(typeof(PlayerData)) as PlayerData) &&
            !IsAlreadyInGroup(collision.gameObject.GetComponent(typeof(PlayerData)) as PlayerData))
        {
            Debug.Log("Et je rentre en collision avec un autre joueur");
            GameManager.instance.closePlayers.Add(collision.gameObject.GetComponent(typeof(PlayerData)) as PlayerData);
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (isLocalPlayer && collision.tag == "Player")
            RemoveFromList(collision.gameObject.GetComponent(typeof(PlayerData)) as PlayerData);
    }

    private void RemoveFromList(PlayerData player)
    {
        for (int i = 0; i < GameManager.instance.closePlayers.Count; i++)
            if (player.pseudo == GameManager.instance.closePlayers[i].pseudo)
            {
                GameManager.instance.closePlayers.RemoveAt(i);
                return;
            }
    }

    private bool IsAlreadyInList(PlayerData player)
    {
        for (int i = 0; i < GameManager.instance.closePlayers.Count; i++)
            if (player.pseudo == GameManager.instance.closePlayers[i].pseudo)
            {
                return true;
            }
        return false;
    }

    private bool IsAlreadyInGroup(PlayerData player)
    {
        int i = 0;
        PlayerData[] array = GroupManager.instance.playersInGroup;
        while (i < 4 && array[i] != null)
        {
            if (player.pseudo == array[i].pseudo)
                return true;
            i++;
        }
        return false;
    }
}