using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable; // permet de définir des cases de collision ex: eau, montagne etc...

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;
    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }
    
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    void OnMouseDown() // Dans cette fonction qu'on peut gérer l'affichage de la portée des coups par exemple
    {
        if(CombatManager.instance.state != CombatState.HeroesTurn) return;

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
                    Destroy(enemy.gameObject);
                    UnitManager.instance.SetSelectedHero(null);
                }
            }
        }
        else
        {
            if (UnitManager.instance.SelectedHero != null) //cas du déplacement
            {
                SetUnit(UnitManager.instance.SelectedHero);
                UnitManager.instance.SetSelectedHero(null);
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
}
