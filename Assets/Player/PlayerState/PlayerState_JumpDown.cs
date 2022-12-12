using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_JumpDown : PlayerState_OnAir
    {
        public override void Enter()
        {
            base.Enter();
            // anim.Play("JumpDown");
            animManager.Play(AnimName.JumpDown);
            if (ctler.ChangeableJump)
                ctler.DecelerationWhenChangeableJump();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //爬上墙
            if (ctler.canWallClimb_Font && ctler.WallSlided_Font)
            {
                FSM.SwitchState(typeof(PlayerState_WallClimb));
            }
            else if (ctler.canWallClimb_Back && ctler.WallSlided_Back)
            {
                ctler.FlipPlayer();
                FSM.SwitchState(typeof(PlayerState_WallClimb));
            }
            //指令缓存跳-在WallSlide中判断就可以
            // else if (ctler.WallSlided_Font && BtnJumpInThisState && input.JumpFrame.IntervalWithLastTrue <= ctler.JumpBufferTime)
            // {
            //     ctler.FlipPlayer();
            //     FSM.SwitchState(typeof(PlayerState_WallJump));
            // }
            //和Idle、Move状态的指令缓存跳重叠
            // else if (ctler.Grounded && input.JumpFrame.IntervalWithLastTrue <= ctler.JumpBufferTime)
            // {
            //     FSM.SwitchState(typeof(PlayerState_JumpUp));
            // }
            //滑落
            //else if ((ctler.WallSlided_Back && input.AxesX * ctler.PlayerFace < 0) || ctler.WallSlided_Font)
            else if (ctler.WallSlided_Back || ctler.WallSlided_Font)
            {
                FSM.SwitchState(typeof(PlayerState_WallSlide));
            }
            //落地
            else if (ctler.Grounded)
            {
                effect.Release落地灰尘();
                if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
