using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random=UnityEngine.Random;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;
    private List<ScriptableUnit> _units;
    public BaseHero SelectedHero;
    [HideInInspector] public int Coups;
    
    private void Awake()
    {
        instance = this;
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    private void Update()
    {
        if (Coups == 2)
        {
            Debug.Log("PASSAGE DANS IF Update UnitManager");
            CombatManager.instance.ChangeState(CombatState.EnemiesTurn);
            return;
        }
    }
    public void SpawnHeroes()
    {
        var heroCount = 1;

        for (int i = 0; i < heroCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);
            spawnedHero.MovesList = new List<Tile>();
            spawnedHero.RangeList = new List<Tile>();
            var randomSpawnTile = GridManager.instance.GetHeroSpawnTile();

            randomSpawnTile.SetUnit(spawnedHero);
        }

        CombatManager.instance.ChangeState(CombatState.SpawnEnemies);
    }

    public void SpawnEnemies()
    {
        var enemyCount = 1;

        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            spawnedEnemy.MovesList = new List<Tile>();
            spawnedEnemy.RangeList = new List<Tile>();
            var randomSpawnTile = GridManager.instance.GetEnemySpawnTile();

            randomSpawnTile.SetUnit(spawnedEnemy);
        }

        CombatManager.instance.ChangeState(CombatState.HeroesTurn);
    }

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T)_units.Where(u => u.faction == faction).OrderBy(o => Random.value).First().unitPrefab;
    }

    public void SetSelectedHero(BaseHero hero)
    {
        SelectedHero = hero;
        MenuManager.instance.ShowSelectedHero(hero);
        GetAvailableTiles(SelectedHero);
    }

    // Fonction qui actualise les listes des coups disponibles
    public void GetAvailableTiles(BaseUnit unit)
    {
        if (unit == null) return;
        ClearLists(unit);
        Vector2 pos = unit.OccupiedTile.Coordonnees;

        //déplacements --> 4 cases autour
        if (pos.x > 0)
            unit.MovesList.Add(GridManager.instance.GetTileAtPosition(new Vector2(pos.x-1,pos.y)));
        if (pos.x < GridManager.instance.Width)
            unit.MovesList.Add(GridManager.instance.GetTileAtPosition(new Vector2(pos.x+1,pos.y)));
        if (pos.y > 0)
            unit.MovesList.Add(GridManager.instance.GetTileAtPosition(new Vector2(pos.x,pos.y-1)));
        if (pos.y < GridManager.instance.Height)
            unit.MovesList.Add(GridManager.instance.GetTileAtPosition(new Vector2(pos.x,pos.y+1)));

        (int limitx, int limity) = ((int)(pos.x+unit.Range.x),(int)(pos.y+unit.Range.y));
        //attaques
        //horizontal --> pos-portée / pos+portée
        for (int i = (int)(pos.x-unit.Range.x); i <= limitx; i++)
        {
            if (i == (int)pos.x) // on n'attaque pas sur place
                continue;
            try
            {
                unit.RangeList.Add(GridManager.instance.GetTileAtPosition(new Vector2(i,pos.y)));
            }
            catch (Exception)
            {
                if (i > pos.x) 
                    break; // index out of range donc sert à rien d'aller plus loin...
                else
                    continue; //index out of range mais le prochain peut-etre pas!
            }
        }
        //vertical
        for (int i = (int)(pos.y-unit.Range.y); i <= limity; i++)
        {
            if (i == (int)pos.y) // on n'attaque pas sur place
                continue;
            try
            {
                unit.RangeList.Add(GridManager.instance.GetTileAtPosition(new Vector2(pos.x,i)));
            }
            catch (Exception)
            {
                if (i > pos.y)
                    break; // index out of range donc sert à rien d'aller plus loin...
                else
                    continue; //index out of range mais le prochain peut-etre pas!
            }
        }
    }

    private static void ClearLists(BaseUnit unit)
    {
        unit.MovesList.Clear();
        unit.RangeList.Clear();
    }
}
