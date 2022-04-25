using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public CombatState state;

    public static event Action<CombatState> OnCombatStateChanged;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeState(CombatState.GenerateGrid);
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
        throw new NotImplementedException();
        // voir à 8:20 https://www.youtube.com/watch?v=4I0vonyqMi8
        // boutons activés uniquement lorsque c'est le tour du joueur
    }
    
    private void HandleEnemiesTurn()
    {
        throw new NotImplementedException();
    }
    
    private void HandleDecide() //après chaque tour, vérifie si la partie est terminée ou non
    {
        throw new NotImplementedException();
    }
    
    private void HandleVictory()
    {
        throw new NotImplementedException();
    }
    
    private void HandleLose()
    {
        throw new NotImplementedException();
    }
}

public enum CombatState
{
    GenerateGrid,
    SpawnHeroes,
    SpawnEnemies,
    HeroesTurn,
    EnemiesTurn,
    Decide,
    Victory,
    Lose
}