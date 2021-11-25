using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Color color;
    [SerializeField]
    private int movementCost;
    [SerializeField]
    private bool passable;

    public Color GetColor()
    {
        return color;
    }

    public int GetMovementCost()
    {
        return movementCost;
    }

    public bool GetPassable()
    {
        return passable;
    }
}
