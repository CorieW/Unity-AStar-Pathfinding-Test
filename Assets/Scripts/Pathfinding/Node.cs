using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Tile tile;

    private Vector2Int tilePos;

    public Node(Tile tile, Vector2Int tilePos)
    {
        this.tile = tile;
        this.tilePos = tilePos;
    }

    public Tile GetTile()
    {
        return tile;
    }

    public Vector2Int GetTilePos()
    {
        return tilePos;
    }

    public Dictionary<Vector2Int, Tile> GetNeightbourTiles()
    {
        Vector2Int tilePosInt = new Vector2Int((int)tilePos.x, (int)tilePos.y);

        Dictionary<Vector2Int, Tile> neighboursDict = new Dictionary<Vector2Int, Tile>();
        neighboursDict.Add(tilePosInt + new Vector2Int(-1, 0), Map.GetInstance().GetTileAt(tilePosInt + new Vector2Int(-1, 0)));
        neighboursDict.Add(tilePosInt + new Vector2Int(1, 0), Map.GetInstance().GetTileAt(tilePosInt + new Vector2Int(1, 0)));
        neighboursDict.Add(tilePosInt + new Vector2Int(0, -1), Map.GetInstance().GetTileAt(tilePosInt + new Vector2Int(0, -1)));
        neighboursDict.Add(tilePosInt + new Vector2Int(0, 1), Map.GetInstance().GetTileAt(tilePosInt + new Vector2Int(0, 1)));
        return neighboursDict;
    }
}
