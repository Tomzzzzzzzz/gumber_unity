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
    [HideInInspector] private bool isWaiting;
    [HideInInspector] private float TimeCounter;
    void Awake()
    {
        instance = this;
        EnemiesAlive = 1; //pour les tests
    }

    private void Start()
    {
        ChangeState(CombatState.GenerateGrid);
        UnitManager.instance.Coups = 0;
        TimeCounter = 0f;
    }

    private void Update()
    {
        if (isWaiting){
            Debug.Log($"Temps = {TimeCounter}");
            if (TimeCounter >= 5f) {
                TimeCounter = 0f;
                isWaiting = false;
                ChangeScene();
            }
            TimeCounter += Time.deltaTime; 
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
        MenuManager.instance._victory.SetActive(true);
        TimeCounter = 0f;
        Debug.Log("Début du compteur !");
        isWaiting = true;
    }
    
    private void HandleLose()
    {
        throw new NotImplementedException();
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("Map");
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