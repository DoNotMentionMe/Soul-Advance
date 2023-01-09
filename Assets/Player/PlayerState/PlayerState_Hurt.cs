using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Hurt : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            //TODO开始受伤-跳一下
            ctler.StartHurt();
            //TODO开始角色闪烁，屏幕闪烁，顿帧，震动
            effect.HurtEffectStart();
            //开始无敌
            ctler.StartHurtNotInjury();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (StateDuration >= ctler.HurtStateTime)
            {
                if (!ctler.Grounded)
                {
                    FSM.SwitchState(typeof(PlayerState_JumpDown));
                }
                else if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else if (!input.Move)
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }

        public override void Exit()
        {
            base.Exit();
            propertyController.GetHurt = false;
            ctler.EndHurt();
        }
    }
}
