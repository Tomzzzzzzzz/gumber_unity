using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] private GameObject _selectedHeroObject, _tileObject, _tileUnitObject;
    [SerializeField] private Button _attack, _moves;
    void Awake()
    {
        instance = this;
    }

    public void ShowTileInfo(Tile tile)
    {
        if (tile == null)
        {
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            return;
        }
        _tileObject.GetComponentInChildren<Text>().text = tile.TileName;
        _tileObject.SetActive(true);

        if(tile.OccupiedUnit)
        {
            _tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName;
            _tileUnitObject.SetActive(true);
        }
    }
    public void ShowSelectedHero(BaseHero hero)
    {
        if (hero == null)
        {
            _selectedHeroObject.SetActive(false);
            /* Trouver comment cacher bouttons
            _attack.SetActive(false);
            _moves.SetActive(false); */
            return;
        }
        _selectedHeroObject.GetComponentInChildren<Text>().text = hero.UnitName;
        _selectedHeroObject.SetActive(true);
        /* Trouver comment afficher bouttons
        _attack.SetActive(true);
        _moves.SetActive(true); */
    }

    // Dans un premier temps, dé-surligne les cases d'attaques (dans tous les cas)
    public void HighlightMovesTiles(BaseHero hero)
    {
        throw new NotImplementedException();
    }

    // Dans un premier temps, dé-surligne les cases de mouvements (dans tous les cas)
    public void HighlightAttackTiles(BaseHero hero)
    {
        throw new NotImplementedException();
    }
}
