using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class SpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject prefabEarth;
    public GameObject prefabLeaf;
    public GameObject prefabWorm;
    
    // Referenced prefabs have to be declared so that the conversion system knows about them ahead of time
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefabLeaf);
        referencedPrefabs.Add(prefabWorm);
    }
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new SpawnerGardenEntity
        {
            prefabEarth = conversionSystem.GetPrimaryEntity(this.prefabEarth),
            prefabWorm = conversionSystem.GetPrimaryEntity(this.prefabWorm),
            prefabLeaf = conversionSystem.GetPrimaryEntity(this.prefabLeaf),
        };
        
        
        dstManager.AddComponentData(entity, spawnerData);
    }
}