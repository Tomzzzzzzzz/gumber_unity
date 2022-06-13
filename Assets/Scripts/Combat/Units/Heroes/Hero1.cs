using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero1 : BaseHero
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public Sprite sprite;
    public bool stopUpdate = false;

    void Awake()
    {
        GridManager.instance.hero = this;
    }
    void Update()
    {
        if (!stopUpdate)
        {
            if (sprite != null)
            {
                spriteRenderer.sprite = sprite;
                stopUpdate = true;
            }
        }
    }
}
