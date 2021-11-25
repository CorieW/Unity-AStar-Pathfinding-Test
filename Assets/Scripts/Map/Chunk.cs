using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Chunk : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Vector2Int chunkPos;
    private Tile[,] tileMap;
    public List<ChunkUser> visibleTo;

    private void Awake()
    {
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();

        tileMap = new Tile[GameSettings.CHUNK_SIZE, GameSettings.CHUNK_SIZE];
        visibleTo = new List<ChunkUser>();
    }

    public void Unload()
    {
        Destroy(gameObject);
    }

    public void SetChunkPos(Vector2Int chunkPos)
    {
        this.chunkPos = chunkPos;
    }

    public void SetTileAt(Tile tile, Vector2Int pos)
    {
        tileMap[pos.x, pos.y] = tile;
    }

    public Vector2Int GetChunkPos()
    {
        return chunkPos;
    }

    public Vector2Int GetChunkTilePos()
    {
        return chunkPos * GameSettings.CHUNK_SIZE;
    }

    public Tile GetTileAt(Vector2Int pos)
    {
        return tileMap[pos.x, pos.y];
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }
}
