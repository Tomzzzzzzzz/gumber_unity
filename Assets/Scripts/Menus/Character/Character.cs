using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    public string characterName;
    public Sprite characterSprite;
    public Sprite headSprite;
    public Sprite combatSprite;

    public static Character instance;

    void Awake()
    {
        instance = this;
    }
}
