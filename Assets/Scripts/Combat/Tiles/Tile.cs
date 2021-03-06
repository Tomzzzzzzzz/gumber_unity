using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] public GameObject _rangeHighlight;
    [SerializeField] public GameObject _movementHighlight;
    [SerializeField] private bool _isWalkable; // permet de définir des cases de collision ex: eau, montagne etc...
    [HideInInspector] public Vector2 Coordonnees;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;
    public virtual void Init(int x, int y)
    {
        this.Coordonnees.x = x;
        this.Coordonnees.y = y;
    }

    void OnMouseEnter()
    {
        if (PauseMenu.GameIsPaused) return;
        _highlight.SetActive(true);
        MenuManager.instance.ShowTileInfo(this);
        if (OccupiedUnit != null && OccupiedUnit.faction == Faction.Enemy)
        {
            UnitManager.instance.panel_hp.SetActive(true);
        }
    }
    
    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.instance.ShowTileInfo(null);
        UnitManager.instance.panel_hp.SetActive(false);
    }

    void OnMouseDown() // Dans cette fonction qu'on peut gérer l'affichage de la portée des coups par exemple
    {
        if (PauseMenu.GameIsPaused) return;
        if(CombatManager.instance.state != CombatState.HeroesTurn || UnitManager.instance.Coups == 2) return;

        if (OccupiedUnit != null)
        {
            if (OccupiedUnit.faction == Faction.Hero)
            {
                UnitManager.instance.SetSelectedHero((BaseHero)OccupiedUnit);
                MenuManager.instance.isShowingSelected = true;
                MenuManager.instance._tileUnitObject.SetActive(false);
            }
            else
            {
                if (UnitManager.instance.SelectedHero != null) // ici on sait qu'on clique sur un ennemi
                {
                    Debug.Log("attaque un ennemi");
                    var enemy = (BaseEnemy) OccupiedUnit;
                    Debug.Log(enemy);
                    // UnitManager.instance.SelectedHero.Attack() ---> dedans on gère l'animation, les pv perdus etc...
                    // Pour l'instant, le kill est instant
                    if (IsInList(this,UnitManager.instance.SelectedHero.RangeList))
                    {
                        Debug.Log("isinlist");
                        MenuManager.instance.UnHighlight(UnitManager.instance.SelectedHero);
                        UnitManager.instance.Attack(UnitManager.instance.SelectedHero, enemy); // Appel sur l'attaque lorsque joueur clique
                        UnitManager.instance.SetSelectedHero(null);
                        UnitManager.instance.Coups += 1;
                    }  
                }
            }
        }
        else //cas du déplacement: this correspond à la case cliquée 
        {
            if (UnitManager.instance.SelectedHero != null && _isWalkable &&
                IsInList(this,UnitManager.instance.SelectedHero.MovesList))
            {
                MenuManager.instance.UnHighlight(UnitManager.instance.SelectedHero);

                SetUnit(UnitManager.instance.SelectedHero);
                UnitManager.instance.GetAvailableTiles(UnitManager.instance.SelectedHero); //actualisation de la liste de coups
                UnitManager.instance.SetSelectedHero(null);
                UnitManager.instance.Coups += 1;
                Debug.Log($"Coup n°{UnitManager.instance.Coups}");
                Console.WriteLine($"Coup n°{UnitManager.instance.Coups}");

                MenuManager.instance.isShowingSelected = false;
                MenuManager.instance._tileUnitObject.SetActive(true);
            }
        }
    }
    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    private static bool IsInList(Tile tile, List<Tile> list)
    {
        foreach (Tile elt in list)
            if (tile.Coordonnees.x == elt.Coordonnees.x && tile.Coordonnees.y == elt.Coordonnees.y)
                return true;
        return false;
    }

    // PARTIE IA //
    // PARTIE IA //
    // PARTIE IA //

 /*   public void EnemyAttack(BaseEnemy enemy)
    {
        if (PauseMenu.GameIsPaused) return;
        if(CombatManager.instance.state != CombatState.EnemiesTurn || UnitManager.instance.Coups == 2) return;
        GetAvailableTiles(enemy);
        if (OccupiedUnit != null)
        {
            if (OccupiedUnit.faction == Faction.Enemy)
            {
                UnitManager.instance.SetSelectedHero((BaseHero)OccupiedUnit);
                MenuManager.instance.isShowingSelected = true;
                MenuManager.instance._tileUnitObject.SetActive(false);
            }
            else
            {
                if (UnitManager.instance.SelectedHero != null) // ici on sait qu'on clique sur un ennemi
                {
                    Debug.Log("attaque un ennemi");
                    var enemy = (BaseEnemy) OccupiedUnit;
                    Debug.Log(enemy);
                    // UnitManager.instance.SelectedHero.Attack() ---> dedans on gère l'animation, les pv perdus etc...
                    // Pour l'instant, le kill est instant
                    if (IsInList(this,UnitManager.instance.SelectedHero.RangeList))
                    {
                        Debug.Log("isinlist");
                        Destroy(enemy.gameObject);
                        MenuManager.instance.UnHighlight(UnitManager.instance.SelectedHero);
                        UnitManager.instance.SetSelectedHero(null);
                        UnitManager.instance.Coups += 1;
                        CombatManager.instance.EnemiesAlive -= 1;
                    }  
                }
            }
        }
    }
*/
}
