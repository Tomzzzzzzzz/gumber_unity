using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit",menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction faction;
    public BaseUnit unitPrefab;
    public static List<ScriptableUnit> ToList(ScriptableUnit[] arr)
    {
        List<ScriptableUnit> result = new List<ScriptableUnit>();
        foreach (var elt in arr)
        {
            result.Add(elt);
        }
        return result;
    }
}

public enum Faction
{
    Hero = 0,
    Enemy = 1
}


