using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    public CombatState state;
    public static event Action<CombatState> OnCombatStateChanged;
    [HideInInspector] public int EnemiesAlive;
    [HideInInspector] public PlayerData player;
    public Sprite sprite;
    public GameObject pseudopanel;
    [HideInInspector] private bool isEnded;
    [HideInInspector] private bool isWaiting;
    [HideInInspector] private float TimeCounter;
    void Awake()
    {
        instance = this;
        EnemiesAlive = 1; //pour les tests
        isEnded = false;
        isWaiting = false;
    }

    private void Start()
    {
        ChangeState(CombatState.GenerateGrid);
        UnitManager.instance.Coups = 0;
    }

    private void Update()
    {
        if (isWaiting)
        {
            if (TimeCounter >= 3f)
            {
                if (isEnded)
                {
                    TimeCounter = 0f;
                    isEnded = false;
                    player.GetComponentInParent<PlayerController>().enabled = true;
                    SceneManager.LoadScene(GameManager.instance.previousZone);
                }
                else
                {
                    TimeCounter = 0f;
                    isEnded = true;
                    Debug.Log("ici ça win de l'xp");
                    MenuManager.instance._victory.SetActive(false);
                    MenuManager.instance._rec.SetActive(true);
                    //si le joueur gagne, lui donner de l'xp
                }
            }
            else
            {
                TimeCounter += Time.deltaTime;
            }
        }
    }

    public void ChangeState(CombatState newstate)
    {
        state = newstate;

        switch (newstate)
        {
            case CombatState.GenerateGrid:
                GridManager.instance.GenerateGrid();
                break;
            case CombatState.SpawnHeroes:
                UnitManager.instance.SpawnHeroes();
                break;
            case CombatState.SpawnEnemies:
                UnitManager.instance.SpawnEnemies();
                break;
            case CombatState.HeroesTurn:
                HandleHeroesTurn();
                break;
            case CombatState.EnemiesTurn:
                HandleEnemiesTurn();
                break;
            case CombatState.Decide:
                HandleDecide();
                break;
            case CombatState.Victory:
                HandleVictory();
                break;
            case CombatState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newstate), newstate, null);
        }

        OnCombatStateChanged?.Invoke(newstate);
    }

    private void HandleHeroesTurn()
    {
        Debug.Log("Appel HandleHeroesTurn");
        UnitManager.instance.Coups = 0;
        // Partie logique déjà gérer par OnMouseDown dans Tile.cs !

        // voir à 8:20 https://www.youtube.com/watch?v=4I0vonyqMi8
        // boutons activés uniquement lorsque c'est le tour du joueur
    }
    
    private void HandleEnemiesTurn()
    {
        Debug.Log("mec !cool passe à l'attaque !!");
        UnitManager.instance.Coups = 0;

        //tant que IA non implémentée

        //Si le monstre est contact avec un/plusieurs joueurs, attaque (le + low hp)
        //Sinon, trouver le joueur le plus proche et se déplacer vers lui et l'attaquer s'il peut
        
        ChangeState(CombatState.Decide);
    }
    
    private void HandleDecide() //après chaque tour, vérifie si la partie est terminée ou non
    {
        if (EnemiesAlive != 0)
        {
            ChangeState(CombatState.HeroesTurn);
            return;
        }
        else
        {

            ChangeState(CombatState.Victory);
            /*
            if (true)
            {
                ChangeState(CombatState.Victory);
                return;
            }
            else
            {
                ChangeState(CombatState.Lose);
                return;
            }*/
        }
    }
    
    private void HandleVictory()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        sprite = player.GetComponentInParent<SpriteRenderer>().sprite;
        TextMeshProUGUI pseudo = pseudopanel.GetComponent<TextMeshProUGUI>();
        int nbxp = (UnityEngine.Random.Range(80,130));
        pseudo.text = player.pseudo + " a gagné " + nbxp.ToString() + " xp !";
        player.AddXp(nbxp);
        foreach (GameObject text in MenuManager.instance.InGameInfo)
        {
            text.SetActive(false);
        }
        Debug.Log("ici ça win");
        MenuManager.instance._victory.SetActive(true); 
        isWaiting = true;
    }
    
    private void HandleLose()
    {
        throw new NotImplementedException();
    }
}

public enum CombatState
{
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    HeroesTurn = 3,
    EnemiesTurn = 4,
    Decide = 5,
    Victory = 6,
    Lose = 7
}