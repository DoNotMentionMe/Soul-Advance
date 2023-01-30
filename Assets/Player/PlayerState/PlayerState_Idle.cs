using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Idle : PlayerState
    {
        private float NotGroundedTime = -100f;
        private bool FirstOnEnable = true;
        private bool FirstExit = true;

        public override void Enter()
        {
            base.Enter();

            //anim.Play("Idle");
            if (!FirstOnEnable)
                animManager.Play(AnimName.Idle);
            FirstOnEnable = false;
            NotGroundedTime = 0f;

            //暂时
            //playerController.Stop();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //按跳跃键或进入状态前按下了跳跃键
            if (input.JumpFrame.Value
                || input.JumpFrame.IntervalWithLastTrue <= ctler.ClimbUpJumpBufferTime)
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
            //踩空，判定为土狼跳或下落
            else if (!ctler.Grounded)
            {
                NotGroundedTime += Time.deltaTime;
                if (NotGroundedTime <= ctler.LeaveGroundJumpBufferTime && input.JumpFrame.Value)
                {
                    FSM.SwitchState(typeof(PlayerState_JumpUp));
                }
                else if (NotGroundedTime > ctler.LeaveGroundJumpBufferTime)
                    FSM.SwitchState(typeof(PlayerState_JumpDown));
            }
            //攻击
            else if (input.AttackFrame.Value)
            {
                FSM.SwitchState(typeof(PlayerState_Attack));
            }
            //翻滚
            else if ((input.RollFrame.Value
                || input.RollFrame.IntervalWithLastTrue <= ctler.RollBufferTime) && propertyController.CanRoll)
            {
                FSM.SwitchState(typeof(PlayerState_Roll));
            }
            //移动
            else if (input.Move)
            {
                FSM.SwitchState(typeof(PlayerState_Move));
            }
        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            //实时加速或减速
            ctler.Move(input.AxesX);
        }

        public override void Exit()
        {
            base.Exit();
            if (FirstExit)
            {
                FirstExit = false;
                ctler.GetOnEnablePos();
            }
        }
    }
}