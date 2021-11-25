using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This uses the Map to generate chunks around it (ChunkUser).
public class ChunkUser : MonoBehaviour
{
    private Vector2Int prevChunkPos;

    private void Start()
    {
        // For loading the chunks initially without a move being required.
        Map.GetInstance().OnChunkUserMoveChunk(this);
    }

    protected virtual void Update()
    {
        OnMove();
    }

    public void OnMove()
    {
        Vector2Int currentChunkPos = GetCurrentChunkPos();
        if (!prevChunkPos.Equals(currentChunkPos)) 
        {
            Map.GetInstance().OnChunkUserMoveChunk(this);
            prevChunkPos = currentChunkPos;
        }
    }

    public virtual void OnChunkLoad()
    {

    }

    public virtual void OnChunkBeforeUnload(Chunk chunk)
    {
        
    }

    public Vector2Int GetCurrentChunkPos()
    {
        Vector2 currentPos = transform.position;
        return new Vector2Int(
            Mathf.RoundToInt(currentPos.x), 
            Mathf.RoundToInt(currentPos.y)
        );
    }

    public Vector2Int GetCurrentTilePos()
    {
        Vector2 currentPos = transform.position;
        return new Vector2Int(
            Mathf.FloorToInt(GameSettings.CHUNK_SIZE * currentPos.x), 
            Mathf.FloorToInt(GameSettings.CHUNK_SIZE * currentPos.y)
        );
    }
}
