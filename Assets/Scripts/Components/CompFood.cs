using System;
using UnityEngine;

namespace Components
{
    public class CompFood : MonoBehaviour
    {
        public event Action<CompFood> OnFinished = (food) => { };

        [SerializeField] private float biteSize = 1f;
        [SerializeField] private float foodAmount = 2f;
    
        public float Eat()
        {
            float currentBiteSize = Mathf.Min(biteSize, foodAmount);
            
            foodAmount -= currentBiteSize;

            if (foodAmount <= 0)
            {
                OnFinished(this);
            }
            
            return currentBiteSize;
        }

    
    }
}
