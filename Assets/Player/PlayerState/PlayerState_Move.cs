using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Move : PlayerState
    {
        private float NotGroundedTime = -100f;
        public override void Enter()
        {
            base.Enter();
            //anim.Play("Walk");
            animManager.Play(AnimName.Walk);
            NotGroundedTime = 0f;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (ctler.canOneWayClimb || ctler.canWallClimb_Font)
            {
                FSM.SwitchState(typeof(PlayerState_WallClimb));
            }
            //按跳跃键或进入状态前按下了跳跃键
            else if (input.JumpFrame.Value
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
            else if (input.RollFrame.Value
                || input.RollFrame.IntervalWithLastTrue <= ctler.RollBufferTime)
            {
                FSM.SwitchState(typeof(PlayerState_Roll));
            }
            //静止站立
            else if (!input.Move)
            {
                FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            //实时加速或减速
            ctler.Move(input.AxesX);
        }
    }
}