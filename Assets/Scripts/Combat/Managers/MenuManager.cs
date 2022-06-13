using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] public GameObject _selectedHeroObject, _tileObject, _tileUnitObject, _range, _moves;
    [SerializeField] public GameObject _victory, _lose;
    [SerializeField] public GameObject _rec;

    [HideInInspector] public List<GameObject> InGameInfo;
    [HideInInspector] public bool isShowingSelected;
    void Awake()
    {
        instance = this;
        InGameInfo = new List<GameObject>() {_selectedHeroObject, _tileObject, _tileUnitObject, _range, _moves};
    }

    void Update()
    {
        if (UnitManager.instance.SelectedHero != null)
        {
            if (Input.GetKeyDown(KeyCode.E)) //Show moves
            {
                ShowMoves(UnitManager.instance.SelectedHero);
            }

            if (Input.GetKeyDown(KeyCode.F)) //Show attacks
            {
                ShowAttack(UnitManager.instance.SelectedHero);
            }
        }
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

        if(tile.OccupiedUnit && !isShowingSelected)
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
            _range.SetActive(false);
            _moves.SetActive(false);
            isShowingSelected = false;
            return;
        }
        _selectedHeroObject.GetComponentInChildren<Text>().text = hero.UnitName;
        _selectedHeroObject.SetActive(true);
        _range.SetActive(true);
        _moves.SetActive(true);
        isShowingSelected = true;
    }

    // Dans un premier temps, dé-surligne les cases d'attaques (dans tous les cas)
    private void HighlightMovesTiles(BaseHero hero)
    {
        foreach (Tile tile in hero.MovesList)
        {
            tile._movementHighlight.SetActive(true);
        }
    }

    // Dans un premier temps, dé-surligne les cases de mouvements (dans tous les cas)
    private void HighlightAttackTiles(BaseHero hero)
    {
        foreach (Tile tile in hero.MovesList)
        {
            tile._rangeHighlight.SetActive(true);
        }
    }

    private void UnHighlightMoves(BaseHero hero)
    {
        foreach (Tile tile in hero.MovesList)
        {
            tile._movementHighlight.SetActive(false);
        }
    }
    private void UnHighlightAttack(BaseHero hero)
    {
        foreach (Tile tile in hero.MovesList)
        {
            tile._rangeHighlight.SetActive(false);
        }
    }

    public void UnHighlight(BaseHero hero)
    {
        UnHighlightMoves(hero);
        UnHighlightAttack(hero);
    }

    private void ShowMoves(BaseHero hero)
    {
        UnHighlightAttack(hero);
        HighlightMovesTiles(hero);
    }
    private void ShowAttack(BaseHero hero)
    {
        UnHighlightMoves(hero);
        HighlightAttackTiles(hero);
    }
}
