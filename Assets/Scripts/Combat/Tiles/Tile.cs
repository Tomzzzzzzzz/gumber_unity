using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _rangeHighlight;
    [SerializeField] private GameObject _movementHighlight;
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
        _highlight.SetActive(true);
        MenuManager.instance.ShowTileInfo(this);
    }
    
    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.instance.ShowTileInfo(null);
    }

    void OnMouseDown() // Dans cette fonction qu'on peut gérer l'affichage de la portée des coups par exemple
    {
        if(CombatManager.instance.state != CombatState.HeroesTurn || UnitManager.instance.Coups == 2) return;

        if (OccupiedUnit != null)
        {
            if (OccupiedUnit.faction == Faction.Hero) UnitManager.instance.SetSelectedHero((BaseHero)OccupiedUnit);
            else
            {
                if (UnitManager.instance.SelectedHero != null) // ici on sait qu'on clique sur un ennemi
                {
                    var enemy = (BaseEnemy) OccupiedUnit;
                    //UnitManager.instance.SelectedHero.Attack() ---> dedans on gère l'animation, les pv perdus etc...
                    // Pour l'instant, le kill est instant
                    if (IsInList(this,UnitManager.instance.SelectedHero.RangeList))
                    {
                        Destroy(enemy.gameObject);
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
                SetUnit(UnitManager.instance.SelectedHero);
                UnitManager.instance.SetSelectedHero(null);
                UnitManager.instance.GetAvailableTiles(UnitManager.instance.SelectedHero); //actualisation de la liste de coups
                UnitManager.instance.Coups += 1;
                Debug.Log($"Coup n°{UnitManager.instance.Coups}");
                Console.WriteLine($"Coup n°{UnitManager.instance.Coups}");
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
            if (Vector2.Equals(tile.Coordonnees,elt.Coordonnees))
                return true;
        return false;
    }
}
