using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    private static Map Instance; // Singleton

    private Dictionary<Vector2Int, Chunk> visibleChunksDict;

    [Header("General Settings")]
    [SerializeField]
    private bool keepNonVisibleChunksLoaded;
    
    [Header("References")]
    [SerializeField]
    private MapGenerator mapGenerator;

    /// <summary>
    /// Initialize the Map.
    /// </summary>
    public void InitializeMap()
    {
        // There should only ever be one instance of GameController.
        if (Instance) Destroy(gameObject);
        else Instance = this;

        int renderDistance = GameSettings.GetInstance().renderDistance;
        visibleChunksDict = new Dictionary<Vector2Int, Chunk>();
    }

    /// <summary>
    /// Runs whenever the ChunkUser moves from one chunk to another.
    /// </summary>
    public void OnChunkUserMoveChunk(ChunkUser chunkUser)
    {
        int renderDistance = GameSettings.GetInstance().renderDistance;
        UnloadAllNonVisibleChunks(chunkUser, renderDistance);

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistance; y <= renderDistance; y++)
            {
                float dist = Mathf.Sqrt((x * x) + (y * y));
                if (dist > renderDistance) continue;

                Vector2Int currentChunkPos = new Vector2Int(x, y) + chunkUser.GetCurrentChunkPos();
                Chunk chunk = null;
                visibleChunksDict.TryGetValue(currentChunkPos, out chunk);
                
                if (chunk) 
                {
                    // Tell the chunk who it is visible to.
                    if (!chunk.visibleTo.Contains(chunkUser)) chunk.visibleTo.Add(chunkUser);
                    continue;
                }

                Chunk newChunk = mapGenerator.GenerateChunk(currentChunkPos);
                newChunk.visibleTo.Add(chunkUser);
                visibleChunksDict.Add(currentChunkPos, newChunk);
            }
        }
        chunkUser.OnChunkLoad();
    }

    /// <summary>
    /// Unloads all of the chunks that are exceeding the renderDistance from centerChunk.
    /// </summary>
    public void UnloadAllNonVisibleChunks(ChunkUser chunkUser, int renderDistance)
    {
        for (int i = 0; i < visibleChunksDict.Keys.Count; i++)
        {
            Vector2Int chunkPos = visibleChunksDict.Keys.ElementAt(i);
            Chunk chunk = visibleChunksDict[chunkPos];

            Vector2Int diffVec = chunkPos - chunkUser.GetCurrentChunkPos();
            float dist = Mathf.Sqrt((diffVec.x * diffVec.x) + (diffVec.y * diffVec.y));
            if (dist <= renderDistance) continue; // Chunk is within render distance, skip.
            if (keepNonVisibleChunksLoaded) continue; // Chunks don't need to be unloaded when this is true.

            if (chunk.visibleTo.Contains(chunkUser))
            {
                chunk.visibleTo.Remove(chunkUser);
                
                if (chunk.visibleTo.Count == 0)
                {
                    chunkUser.OnChunkBeforeUnload(chunk);
                    chunk.Unload();
                    visibleChunksDict.Remove(chunkPos);
                    i -= 1;
                }
            }
        }
    }

    public Tile GetTileAt(Vector2Int tilePos)
    {
        // Floor(-15 / 16) = 1, Floor(22 / 16) = 1
        Vector2Int chunkPos = new Vector2Int(Mathf.FloorToInt((float)tilePos.x / GameSettings.CHUNK_SIZE), Mathf.FloorToInt((float)tilePos.y / GameSettings.CHUNK_SIZE));
        Chunk chunk = null;
        visibleChunksDict.TryGetValue(chunkPos, out chunk);
        //Debug.Log(new Vector2Int(Mathf.Abs(tilePos.x) - (chunkPos.x * GameSettings.CHUNK_SIZE), Mathf.Abs(tilePos.y) - (chunkPos.y * GameSettings.CHUNK_SIZE)));

        // Abs(-15 - 0) = 15, 22 - (1 * 16) = 6
        if (chunk) return chunk.GetTileAt(new Vector2Int(Mathf.Abs(tilePos.x - (chunkPos.x * GameSettings.CHUNK_SIZE)), Mathf.Abs(tilePos.y - (chunkPos.y * GameSettings.CHUNK_SIZE))));
        else return null;
    }

    public static Map GetInstance()
    {
        return Instance;
    }
}