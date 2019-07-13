
using UnityEngine;

namespace Entities
{
    class SilkWorm : EntityBase
    {
        public float fullness;

        private SilkWormConfig wormConfig;

        public override void Init()
        {
            this.wormConfig = Root.ConfigManager.SilkWormConfig;

            fullness = wormConfig.startFullness;
        }
        


        private void Update()
        {
            fullness -= wormConfig.hungerSpeed * Time.deltaTime;
            
            OnDebugInfoUpdate($"fulness: {fullness}");
            
            if (fullness <= 0)
            {
                Die();
            }

            
        }

    }
}