using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    public CombatState state;
    public static event Action<CombatState> OnCombatStateChanged;
    [HideInInspector] public int EnemiesAlive;
    [HideInInspector] private bool isEnded;
    [HideInInspector] private float TimeCounter;
    void Awake()
    {
        instance = this;
        EnemiesAlive = 1; //pour les tests
        isEnded = false;
    }

    private void Start()
    {
        ChangeState(CombatState.GenerateGrid);
        UnitManager.instance.Coups = 0;
    }

    private void Update()
    {
        if (isEnded)
        {
            if (TimeCounter >= 3f)
            {
                TimeCounter = 0f;
                isEnded = false;
                SceneManager.LoadScene(GameManager.instance.previousZone);
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
        foreach (GameObject text in MenuManager.instance.InGameInfo)
        {
            text.SetActive(false);
        }
        Debug.Log("ici ça win");
        MenuManager.instance._victory.SetActive(true);
        isEnded = true;
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