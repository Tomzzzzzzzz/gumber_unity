using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public string UnitName;
    public Tile OccupiedTile;
    public Faction faction;
    [SerializeField] public Vector2 Range;
    [HideInInspector] public List<Tile> MovesList;
    [HideInInspector] public List<Tile> RangeList;
}
