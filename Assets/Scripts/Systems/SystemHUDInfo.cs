using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

public class SystemHUDInfo : ComponentSystem
{
    public struct HUDInfo
    {
        public Vector3 position;
        public string infoString;
    }
    
    protected override void OnUpdate()
    {
        
        List<HUDInfo> hudInfos = new List<HUDInfo>(); 
        
        Entities.WithAllReadOnly<ComponentEater>().ForEach(
            (Entity id, ref ComponentEater componentEater, ref Translation translation) =>
            {
                hudInfos.Add(new HUDInfo
                {
                    position = translation.Value,
                    infoString = $"{componentEater.currentFullness:0.#}"
                });

            }
        );
        
        Entities.WithAllReadOnly<ComponentFood>().ForEach(
            (Entity id, ref ComponentFood componentFood, ref Translation translation) =>
            {
                hudInfos.Add(new HUDInfo
                {
                    position = translation.Value,
                    infoString = $"{componentFood.foodAmount:0.#}"
                });

            }
        );
        
        Root.UIManager.ShowHUDInfos(hudInfos);
    }
}