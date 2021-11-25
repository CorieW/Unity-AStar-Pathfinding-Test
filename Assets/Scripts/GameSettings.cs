using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private static GameSettings Instance; // Singleton

    public const int CHUNK_SIZE = 16;

    [Header("Render Settings")]
    [SerializeField]
    public int renderDistance;

    /// <summary>
    /// Initialize the GameSettings.
    /// </summary>
    public void InitializeGameSettings()
    {
        // There should only ever be one instance of GameSettings.
        if (Instance) Destroy(gameObject);
        else Instance = this;
    }

    public static GameSettings GetInstance()
    {
        return Instance;
    }
}
