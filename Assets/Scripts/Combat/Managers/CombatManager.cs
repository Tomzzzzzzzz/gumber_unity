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
        UnitManager.instance.Coups = 0;
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
        if (true)
        {
            ChangeState(CombatState.HeroesTurn);
            return;
        }
        else
        {
            if (true)
            {
                ChangeState(CombatState.Victory);
                return;
            }
            else
            {
                ChangeState(CombatState.Lose);
                return;
            }
        }
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
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    HeroesTurn = 3,
    EnemiesTurn = 4,
    Decide = 5,
    Victory = 6,
    Lose = 7
}