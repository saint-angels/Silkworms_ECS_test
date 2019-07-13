using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform hudContainer = null;
    [SerializeField] private HUDBase hudPrefab;
    
    private AnimationConfig animationCfg;


    private readonly Dictionary<EntityBase, HUDBase> entityHuds = new Dictionary<EntityBase, HUDBase>();
    
    

    public void Init()
    {
        animationCfg = Root.ConfigManager.Animation;
    }


    public void SetHUDForEntity(EntityBase entity)
    {
        var newEntityHud = ObjectPool.Spawn(hudPrefab, Vector3.zero,  Quaternion.identity, hudContainer);
        newEntityHud.SetOwner(entity);
        
        entityHuds.Add(entity, newEntityHud);
        
        entity.OnDeath += EntityOnOnDeath;
    }

    private void EntityOnOnDeath(EntityBase entity)
    {
        entity.OnDeath -= EntityOnOnDeath;
        ObjectPool.Despawn(entityHuds[entity]);
        entityHuds.Remove(entity);
    }


    private void LateUpdate()
    {
        foreach (var entity in entityHuds.Keys)
        {
            Vector2 screenPoint = Root.CameraController.WorldToScreenPoint(entity.HUDPoint.position);
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hudContainer, screenPoint, null, out localPoint))
            {
                entityHuds[entity].transform.localPosition = localPoint;
            }
        }
    }
}
