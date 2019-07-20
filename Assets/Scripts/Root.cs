using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Root : MonoBehaviour
{
    public static ConfigManager ConfigManager => _instance.configManager;
    public static CameraController CameraController => _instance.cameraController;
    public static UIManager UIManager => _instance.uiManager;
    
    public static PlayerInput PlayerInput => _instance.playerInput;

    public static SpawnManager SpawnManager => _instance.spawnManager;
    
    [SerializeField] private ConfigManager configManager = null;
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private SpawnManager spawnManager = null;

    private static Root _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        spawnManager.Init();
        
        cameraController.Init();
        uiManager.Init();
    }

    
}
