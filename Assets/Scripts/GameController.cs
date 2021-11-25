using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController Instance; // Singleton

    [Header("References")]
    [SerializeField]
    private GameSettings gameSettings;
    [SerializeField]
    private Map map;

    private void Awake()
    {
        // There should only ever be one instance of GameController.
        if (Instance) Destroy(gameObject);
        else Instance = this;

        gameSettings.InitializeGameSettings();
        map.InitializeMap();
    }

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public static GameController GetInstance()
    {
        return Instance;
    }
}
