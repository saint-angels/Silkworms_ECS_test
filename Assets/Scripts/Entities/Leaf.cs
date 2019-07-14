using System;
using Components;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(CompFood))]
    class Leaf : EntityBase
    {
        [SerializeField] private CompFood componentFood = null;

        private void Awake()
        {
            componentFood = GetComponent<CompFood>();
            componentFood.OnFinished += (f) => Die();
        }

        public override void Init()
        {
            
        }
    }
}