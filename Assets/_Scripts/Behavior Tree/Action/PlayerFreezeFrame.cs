using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("玩家攻击命中表现,特效、降低玩家的动画速度和移速,FreezeFrameTime时间后恢复")]
    public class PlayerFreezeFrame : Action
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
