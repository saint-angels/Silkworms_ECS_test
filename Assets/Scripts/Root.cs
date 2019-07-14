﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public static ConfigManager ConfigManager => _instance.configManager;
    public static CameraController CameraController => _instance.cameraController;
    public static UIManager UIManager => _instance.uiManager;
    
    public static PlayerInput PlayerInput => _instance.playerInput;

    public static EntitiesTracker EntitiesTracker => _instance.entitiesTracker;

    [SerializeField] private ConfigManager configManager = null;
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private EntitiesTracker entitiesTracker = null;

    private static Root _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        cameraController.Init();
        uiManager.Init();

        foreach (var spawner in FindObjectsOfType<EntitiesSpawner>())
        {
            spawner.Init();
        }
    }

    
}
