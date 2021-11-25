using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goal
{
    [SerializeField]
    private Vector2Int tilePos;

    public Goal(Vector2Int tilePos)
    {
        this.tilePos = tilePos;
    }

    public Vector2Int GetTilePos()
    {
        return tilePos;
    }
}
