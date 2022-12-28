using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("玩家命中特效")]
    public class EffectHittedByPlayer : Action
    {
        private PlayerEffectPerformance playerEffect;

        public override void OnAwake()
        {
            playerEffect = PlayerFSM.Player.effect;
        }

        public override TaskStatus OnUpdate()
        {
            playerEffect.AttackHittedEffect();
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            playerEffect = null;
        }
    }
}
