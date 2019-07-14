
using System;
using Components;
using UnityEngine;

namespace Entities
{
    class SilkWorm : EntityBase
    {
        enum State
        {
            SEARCHING,
            EATING
        }
        
        public float fullness;

        private SilkWormConfig wormConfig;

        private State wormState;

        private float foodCheckCooldown = 1f;
        private float foodCheckCooldownCurrent = 0f;

        private CompFood currentFood = null;
        

        public override void Init()
        {
            this.wormConfig = Root.ConfigManager.SilkWormConfig;

            fullness = wormConfig.startFullness;

            wormState = State.SEARCHING;
        }


        private void Update()
        {

            switch (wormState)
            {
                case State.SEARCHING:
                    if (foodCheckCooldownCurrent <= 0)
                    {
                        var foodList = Root.EntitiesTracker.GetFood();
                        foreach (CompFood compFood in foodList)
                        {
                            bool closeEnough = Vector3.Distance(compFood.transform.position, transform.position) < 1f;
                            if (closeEnough)
                            {
                                currentFood = compFood;
                                wormState = State.EATING;
                                break;
                            }
                        }

                        foodCheckCooldownCurrent = foodCheckCooldown;
                    }
                    else
                    {
                        fullness -= wormConfig.hungerSpeed * Time.deltaTime;
                        foodCheckCooldownCurrent -= Time.deltaTime;
                    }
                    
                    break;
                case State.EATING:
                    if (currentFood != null)
                    {
                        fullness += currentFood.Eat();
                    }
                    break;
            }

            OnDebugInfoUpdate($"{wormState} {fullness:0.#}");
            
            
            if (fullness <= 0)
            {
                Die();
            }
        }

    }
}