﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Root : MonoBehaviour
{
    public static float SimulationTick => _instance.simulationTick;

    public static ConfigManager ConfigManager => _instance.configManager;
    public static CameraController CameraController => _instance.cameraController;
    public static UIManager UIManager => _instance.uiManager;
    
    public static PlayerInput PlayerInput => _instance.playerInput;

   
    [SerializeField] private ConfigManager configManager = null;
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private PlayerInput playerInput = null;
    [Range(0f, 10f)]
    [SerializeField] private float simulationTick = .5f;
        
    private static Root _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        cameraController.Init();
        uiManager.Init();
    }

    
}
