using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random=UnityEngine.Random;
using TMPro;
using UnityEngine.UI;
using Mirror;
public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;
    private List<ScriptableUnit> _units;
    public BaseHero SelectedHero;
    [HideInInspector] public int Coups;

    public GameObject panel_hp;
    public Text hp_mob;
    
    private void Awake()
    {
        instance = this;
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    private void FixedUpdate()
    {
        hp_mob.text = CombatManager.instance.enemy.hp + "/100";
        if (Coups == 2)
        {
            Debug.Log("PASSAGE DANS IF Update UnitManager");
            CombatManager.instance.ChangeState(CombatState.EnemiesTurn);
            return;
        }
    }
    public void SpawnHeroes()
    {
        //var heroCount = CombatManager.instance.HeroesAlive;
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
        var enemyCount = CombatManager.instance.EnemiesAlive;
        //var enemyCount = 1;

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
        if (pos.x < GridManager.instance.Width - 1)
            unit.MovesList.Add(GridManager.instance.GetTileAtPosition(new Vector2(pos.x+1,pos.y)));
        if (pos.y > 0)
            unit.MovesList.Add(GridManager.instance.GetTileAtPosition(new Vector2(pos.x,pos.y-1)));
        if (pos.y < GridManager.instance.Height - 1)
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

    public Tile CanAttack(BaseUnit unit)
    {
        foreach(Tile tile in unit.RangeList)
        {
            if (tile != null && tile.OccupiedUnit != null && tile.OccupiedUnit.faction == Faction.Hero)
                return tile;
        }
        return null;
    }

    public void Attack(BaseUnit attacker, BaseUnit attacked)
    {
        //Animation de l'attacker ?? @Nico
        Debug.Log(attacker + " attaque " + attacked);
        attacked.hp -= attacker.damage;
        if (attacked.faction == Faction.Enemy)
        {
            //hp_mob.text = attacked.hp + "/100";
        }
        else
        {
            NetworkClient.localPlayer.gameObject.GetComponent<PlayerData>().currentHealth -= attacker.damage;
        }
        
        if (attacked.hp <= 0)
        {
            if (attacked.faction == Faction.Hero)
            {
                CombatManager.instance.HeroesAlive -= 1;
            }
            else
            {
                CombatManager.instance.EnemiesAlive -= 1;
            }

            if (CombatManager.instance.HeroesAlive == 0)
            {
                Debug.Log("Défaite?");
                CombatManager.instance.HandleLose();
            }
            else if (CombatManager.instance.EnemiesAlive == 0)
            {
                Debug.Log("Victoire?");
                CombatManager.instance.HandleVictory();
            }
        }
    }

    public void MoveEnemy(BaseEnemy enemy)
    {
        bool moved = false;
        int distX = (int) (enemy.transform.position.x - GridManager.instance.hero.transform.position.x);
        int distY = (int) (enemy.transform.position.y - GridManager.instance.hero.transform.position.y);
        int posX = distX < 0 ? distX * -1 : distX;
        int posY = distY < 0 ? distY * -1 : distY;
        if (distX == 0) // Cas sur la même colonne
        {
            try
            {
                Tile tile;
                if (distY > 0)
                {
                    tile = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x, enemy.transform.position.y - 1));
                }
                else
                {
                    tile = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x, enemy.transform.position.y + 1));
                }
                if (tile.Walkable)
                {
                    tile.SetUnit(enemy);
                    moved = true;
                }
            
            }
            catch (Exception)
            {
                Debug.Log("IA : Accès à tile inexistante");
            }
        }
        if (distY == 0 && !moved) // Cas sur la même ligne
        {
            try
            {
                Tile tile;
                if (distX > 0)
                {
                    tile = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y));
                }
                else
                {
                    tile = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y));
                }
                if (tile.Walkable)
                {
                    tile.SetUnit(enemy);
                    moved = true;
                }
            
            }
            catch (Exception)
            {
                Debug.Log("IA : Accès à tile inexistante");
            }
        }

        if (posX < posY && !moved)
        {
            try
            {
                Tile tile;
                if (distX < 0)
                {
                    tile  = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y));
                }
                else
                {
                    tile = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y));
                }
                if (tile.Walkable)
                {
                    tile.SetUnit(enemy);
                    moved = true;
                }
            
            }
            catch (Exception)
            {
                Debug.Log("IA : Accès à tile inexistante");
            }
        }
        if (!moved)
        {
            try
            {
                Tile tile;
                if (distY < 0)
                {
                    tile = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x, enemy.transform.position.y + 1));
                }
                else
                {
                    tile = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x, enemy.transform.position.y - 1));
                }
                
                if (tile.Walkable)
                {
                    tile.SetUnit(enemy);
                    moved = true;
                }
                if (!moved)
                {
                    tile = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y));
                    if (tile.Walkable)
                    {
                        tile.SetUnit(enemy);
                        moved = true;
                    }
                }
                
            }
            catch (Exception)
            {
                Debug.Log("IA : Accès à tile inexistante");
            }
        }
        if (!moved)
        {
            try
            {
                Tile tile = GridManager.instance.GetTileAtPosition(new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y));
                if (tile.Walkable)
                {
                    tile.SetUnit(enemy);
                    moved = true;
                }
                else
                {
                    // IA ne peut pas bouger ce gros débilos
                    CombatManager.instance.state = CombatState.Victory;
                }
            
            }
            catch (Exception)
            {
                Debug.Log("IA : Accès à tile inexistante");
                CombatManager.instance.state = CombatState.Victory;
            }
        }

    }
}
