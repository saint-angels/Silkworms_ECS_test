using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntitiesSpawner : MonoBehaviour
{
    [SerializeField] private EntityType entityType;

    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private float spawnCooldown = 5f;
    [SerializeField] private int burstSize = 1;
    

    public void Init()
    {
        StartCoroutine(SpawnRoutine());
    }


    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            for (int i = 0; i < burstSize; i++)
            {
                EntityBase entityBase = Root.EntitiesTracker.Spawn(entityType);
                if (entityBase != null)
                {
                    entityBase.transform.position = Random.insideUnitCircle * spawnRadius;
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
