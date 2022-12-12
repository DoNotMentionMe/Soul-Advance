using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Roll : PlayerState
    {
        //private float NotGroundedTime = -100f;

        public override void Enter()
        {
            base.Enter();
            effect.Release落地灰尘();
            ctler.RollStart(input.AxesX);
            animManager.CrossFade(AnimName.Roll);
            //NotGroundedTime = 0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //跳跃
            if (input.JumpFrame.Value && ctler.Grounded)
            {
                input.JumpFrame.TrueWhenGrounded = true;
                FSM.SwitchState(typeof(PlayerState_JumpUp));
            }
            else if (input.AttackFrame.Value)
            {
                FSM.SwitchState(typeof(PlayerState_Attack));
            }
            //踩空，判定为土狼跳或下落
            // else if (!ctler.Grounded)
            // {
            //     NotGroundedTime += Time.deltaTime;
            //     if (NotGroundedTime <= ctler.LeaveGroundJumpBufferTime && input.JumpFrame.Value)
            //     {
            //         FSM.SwitchState(typeof(PlayerState_JumpUp));
            //     }
            //     else if (NotGroundedTime > ctler.LeaveGroundJumpBufferTime)
            //         FSM.SwitchState(typeof(PlayerState_JumpDown));
            // }
            //翻滚结束，进入站立或下落状态
            else if (animManager.IsAnimEnded(AnimName.Roll))
            {
                if (!ctler.Grounded)
                {
                    // NotGroundedTime += Time.deltaTime;
                    // if (NotGroundedTime <= ctler.LeaveGroundJumpBufferTime && input.JumpFrame.Value)
                    // {
                    //     FSM.SwitchState(typeof(PlayerState_JumpUp));
                    // }
                    // else if (NotGroundedTime > ctler.LeaveGroundJumpBufferTime)
                    FSM.SwitchState(typeof(PlayerState_JumpDown));
                }
                else if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else if (!input.Move)
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();

            //翻滚移速判定
            ctler.Rolling(input.AxesX);
        }
    }
}
