using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField]
    private int seed;
    [SerializeField]
    private float scale;

    [Header("Tile Settings")]
    [SerializeField]
    private Tile grassTile;
    [SerializeField]
    private Tile sandTile;
    [SerializeField]
    private Tile waterTile;

    [Header("References")]
    [SerializeField]
    private Transform chunksParentTransform;
    [SerializeField]
    private Chunk chunkPrefab;

    public Chunk GenerateChunk(Vector2Int pos)
    {
        Chunk newChunk = Instantiate(chunkPrefab, Vector3.zero, Quaternion.identity);
        newChunk.SetChunkPos(pos);
        newChunk.gameObject.name = $"Chunk ({pos.x}, {pos.y})";
        newChunk.transform.SetParent(chunksParentTransform);
        newChunk.transform.position = new Vector2(pos.x, pos.y);

        SpriteRenderer chunkSR = newChunk.GetSpriteRenderer();
        Texture2D texture2D = new Texture2D(GameSettings.CHUNK_SIZE, GameSettings.CHUNK_SIZE);
        texture2D.filterMode = FilterMode.Point;

        Color[] colors = new Color[texture2D.width * texture2D.height];
        for (int tY = 0, i = 0; tY < texture2D.height; tY++)
        {
            for (int tX = 0; tX < texture2D.width; tX++, i++)
            {
                Vector2 noisePos = new Vector2((float)(seed + (pos.x * GameSettings.CHUNK_SIZE) + tX) / scale, (float)(seed + (pos.y * GameSettings.CHUNK_SIZE) + tY) / scale);
                float noise = Mathf.Lerp(0, 1, Mathf.PerlinNoise(noisePos.x, noisePos.y));

                void AddTile(Tile tile)
                {
                    newChunk.SetTileAt(tile, new Vector2Int(tX, tY));
                    colors[i] = tile.GetColor();
                }

                if (noise <= 0.5f)
                {
                    AddTile(grassTile);
                }
                else if (noise <= 0.6f)
                {
                    AddTile(sandTile);
                }
                else
                {
                    AddTile(waterTile);
                }
            }
        }
        texture2D.SetPixels(colors);
        texture2D.Apply();
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0f, 0f), 16);
        chunkSR.sprite = sprite;

        return newChunk;
    }
}
