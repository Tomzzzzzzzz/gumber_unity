using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using Mirror;
public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    public CombatState state;
    public static event Action<CombatState> OnCombatStateChanged;
    [HideInInspector] public int EnemiesAlive, HeroesAlive;
    [HideInInspector] public PlayerData player;
    public Sprite sprite;
    public GameObject pseudopanel;
    [HideInInspector] private bool isEnded;
    [HideInInspector] private bool isWaiting;
    [HideInInspector] private float TimeCounter;

    [HideInInspector] private bool IAwait;
    [HideInInspector] private float IAwaitCount;
    public Enemy1 enemy;
    void Awake()
    {
        instance = this;
        EnemiesAlive = 1; //pour les tests
        HeroesAlive = 1;
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
        if (IAwait)
        {
            if (IAwaitCount >= 2f)
            {
                IAwait = false;
                IAwaitCount = 0f;
                HandleEnemiesTurn();
            }
            else
            {
                IAwaitCount += Time.deltaTime;
            }
        }
        if (isWaiting)
        {
            if (TimeCounter >= 3f)
            {
                if (isEnded || state == CombatState.Lose)
                {
                    TimeCounter = 0f;
                    isEnded = false;
                    NetworkClient.localPlayer.GetComponent<PlayerController>().disabled = false;
                    NetworkClient.localPlayer.GetComponent<PlayerData>().currentHealth = NetworkClient.localPlayer.GetComponent<PlayerData>().maxHealth;
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
                //IAwait = true;
                HandleEnemiesTurn();//à remettre si on ne veut plus attendre => supprimer IAwait
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
        UnitManager.instance.GetAvailableTiles(enemy);
        Tile possible_attack = UnitManager.instance.CanAttack(enemy);
        if (possible_attack != null)
        {
            Debug.Log("IA : Attaque possible");
            UnitManager.instance.Attack(enemy, possible_attack.OccupiedUnit);
            UnitManager.instance.Coups += 1;
        }
        else
        {
            Debug.Log("IA : Essaie de se déplacer");
            UnitManager.instance.MoveEnemy(enemy);
            UnitManager.instance.Coups += 1;
        }
        //Si le monstre est contact avec un/plusieurs joueurs, attaque (le + low hp)
        //Sinon, trouver le joueur le plus proche et se déplacer vers lui et l'attaquer s'il peut
        ChangeState(CombatState.Decide);
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }
    private void HandleDecide() //après chaque tour, vérifie si la partie est terminée ou non
    {
        if (EnemiesAlive == 0)
        {
            ChangeState(CombatState.Victory);
        }
        else if (HeroesAlive == 0)
        {
            ChangeState(CombatState.Lose);
        }
        else
        {
            Debug.Log($"Enemies alive = {EnemiesAlive}");
            ChangeState(CombatState.HeroesTurn);
        }
    }
    
    public void HandleVictory()
    {
        Debug.Log("oui oui victoire");
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
    
    public void HandleLose()
    {
        Debug.Log("oui oui défaite");
        foreach (GameObject text in MenuManager.instance.InGameInfo)
        {
            text.SetActive(false);
        }
        MenuManager.instance._lose.SetActive(true);
        isWaiting = true;
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