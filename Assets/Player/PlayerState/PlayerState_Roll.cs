using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Roll : PlayerState
    {
        //private float NotGroundedTime = -100f;
        private bool CountJumpBuffering = false;
        private bool RollEnd = false;
        private float IsNotGroundTime;

        public override void Enter()
        {
            base.Enter();
            animManager.CrossFade(AnimName.Roll);
            effect.Release落地灰尘();
            ctler.RollStart(input.AxesX);
            //NotGroundedTime = 0f;
            if (!ctler.Grounded)//在空中Roll时不能再跳跃
            {
                CountJumpBuffering = true;
                IsNotGroundTime = -100;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!CountJumpBuffering && !ctler.Grounded)//模拟土狼跳
            {
                CountJumpBuffering = true;
                IsNotGroundTime = Time.time;
            }
            //跳跃
            if (input.JumpFrame.Value && !ctler.HeadTouchGround)
            {
                if (ctler.Grounded)
                {
                    if (ctler.GroundedOneWay && input.AxesY < 0)
                    {
                        ctler.OneWayDownFall(null);
                        FSM.SwitchState(typeof(PlayerState_JumpDown));
                    }
                    else
                    {
                        input.JumpFrame.TrueWhenGrounded = true;
                        FSM.SwitchState(typeof(PlayerState_JumpUp));
                    }
                }
                else if (CountJumpBuffering && Time.time - IsNotGroundTime <= ctler.ClimbUpJumpBufferTime)
                {
                    input.JumpFrame.TrueWhenGrounded = true;
                    FSM.SwitchState(typeof(PlayerState_JumpUp));
                }
            }
            else if (input.AttackFrame.Value && !ctler.HeadTouchGround)
            {
                FSM.SwitchState(typeof(PlayerState_Attack));
            }
            //翻滚结束，进入站立或下落状态
            else if (animManager.IsAnimEnded(AnimName.Roll))
            {
                RollEnd = true;
                if (ctler.HeadTouchGround)
                {
                    ctler.RollHold();
                }
                else if (!ctler.Grounded)
                {
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
            if (!RollEnd)
                ctler.Rolling(input.AxesX);
        }

        public override void Exit()
        {
            base.Exit();
            CountJumpBuffering = false;
            RollEnd = false;
            ctler.RollEnd();
        }
    }
}
