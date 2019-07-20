using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerAuto : MonoBehaviour
{
    [SerializeField] private EntityType entityType;
    
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private float spawnCooldown = 5f;
    [SerializeField] private int burstSize = 1;

    [SerializeField] private bool doSpawn = true;
    
    private Vector2[][] spawnPatterns = new Vector2[][]
    {
        new Vector2[]
        {
            new Vector2(-1f, -1f),
            new Vector2(0, 0f),
            new Vector2(1f, 1f),
            
        } 
    };

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }


    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (doSpawn)
            {
                for (int i = 0; i < burstSize; i++)
                {
                    Vector2 instancePosition = Random.insideUnitCircle * spawnRadius;
                    Vector2 position = transform.TransformPoint(instancePosition);
                    Root.SpawnManager.SpawnEntity(entityType, position);
                }    
            }
            
            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
