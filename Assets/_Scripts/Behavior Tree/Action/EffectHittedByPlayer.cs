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
        [SerializeField] SharedFloat FreezeFrameTime;
        private PlayerEffectPerformance playerEffect;

        public override void OnAwake()
        {
            playerEffect = PlayerFSM.Player.effect;
        }

        public override TaskStatus OnUpdate()
        {
            playerEffect.AttackHittedEffect(FreezeFrameTime.Value);
            return TaskStatus.Success;
        }
    }
}
