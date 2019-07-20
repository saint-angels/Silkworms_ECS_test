using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerManual : MonoBehaviour
{
    [SerializeField] private EntityType entityType;
    
    [SerializeField] private float positionsMultiplier = 3f;
    
    private Vector2[][] spawnPatterns = new Vector2[][]
    {
        new Vector2[]
        {
            new Vector2(-1f, -1f),
            new Vector2(0, 0f),
            new Vector2(1f, 1f),
            
        } 
    };

    public void Spawn()
    {
        int randPatternIndex = Random.Range(0, spawnPatterns.Length);
        Vector2[] spawnPattern = spawnPatterns[randPatternIndex];

        foreach (Vector2 spawnPoint in spawnPattern)
        {
            Vector2 worldSpawnPoint = transform.TransformPoint(spawnPoint);
            Root.SpawnManager.SpawnEntity(entityType, worldSpawnPoint);
        }
        
    }
    
}
