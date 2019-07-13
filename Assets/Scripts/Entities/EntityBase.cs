using System;
using UnityEngine;

namespace Entities
{
    public abstract class EntityBase : MonoBehaviour
    {
        public event Action<EntityBase> OnDeath = (entity) => { };
        public Action<string> OnDebugInfoUpdate = (infoString) => { };
        
        public Transform HUDPoint => hudPoint;
        
        [SerializeField] private Transform hudPoint = null;
        
        public abstract void Init();

        protected virtual void Die()
        {
            OnDeath(this);
            ObjectPool.Despawn(this);
        }
    }
}