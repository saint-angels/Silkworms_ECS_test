using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntitiesSpawner : MonoBehaviour
{
    [SerializeField] private EntityBase entityPrefab;

    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private float spawnCooldown = 5f;

    public void Init()
    {
        StartCoroutine(SpawnRoutine());
    }


    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Vector3 spawnPosition = Random.insideUnitCircle * spawnRadius;
            
            var newEntity = ObjectPool.Spawn(entityPrefab, spawnPosition, Quaternion.identity);
            newEntity.Init();
            
            
            Root.UIManager.SetHUDForEntity(newEntity);
            
            
            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
