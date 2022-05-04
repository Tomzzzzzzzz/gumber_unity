using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _grassTile, _waterTile;
    [SerializeField] private Transform _cam;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private Sprite playerSprite;
    public int Width => _width;
    public int Height => _height;

    private Dictionary<Vector2, Tile> _tiles;

    private void Awake()
    {
        instance = this;
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var randomTile = Random.Range(0,6) == 3 ? _waterTile : _grassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.Init(x,y);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        
        _cam.transform.position = new Vector3((float) _width / 2 -0.5f, (float) _height / 2 -0.5f,-10);
        
        CombatManager.instance.ChangeState(CombatState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile()
    {
        return _tiles.Where(t=>t.Key.x < _width/2 && t.Value.Walkable).OrderBy(t=>Random.value).First().Value;
    }

    public Tile GetEnemySpawnTile()
    {
        return _tiles.Where(t=>t.Key.x > _width/2 && t.Value.Walkable).OrderBy(t=>Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
}
