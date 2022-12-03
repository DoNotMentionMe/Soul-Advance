using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_JumpUp : PlayerState_OnAir
    {
        public AnyPortrait.apAnimPlayData JumpUp;

        public override void Enter()
        {
            base.Enter();
            //anim.Play("JumpUp");
            if (JumpUp != null)
                apPortrait.Play(JumpUp);
            else
                JumpUp = apPortrait.Play("JumpUp");
            ctler.Jump();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //爬上墙，如果按着跳跃键就不判定
            if (ctler.canWallClimb_Font && ctler.WallSlided_Font && !input.Jump)
            {
                FSM.SwitchState(typeof(PlayerState_WallClimb));
            }
            else if (ctler.canWallClimb_Back && ctler.WallSlided_Back && !input.Jump)
            {
                ctler.FlipPlayer();
                FSM.SwitchState(typeof(PlayerState_WallClimb));
            }
            //指令缓存跳-在WallSlide中判断就可以
            // else if (ctler.WallSlided_Font && BtnJumpInThisState && input.JumpFrame.IntervalWithLastTrue <= ctler.WallJumpBufferTimeWithnOnAir)
            // {
            //     ctler.FlipPlayer();
            //     FSM.SwitchState(typeof(PlayerState_WallJump));
            // }
            //滑落
            else if ((ctler.WallSlided_Back || ctler.WallSlided_Font) && !input.Jump)
            {
                FSM.SwitchState(typeof(PlayerState_WallSlide));
            }
            //JumpDown or 可变跳
            else if (ctler.JumpDown || (ctler.ChangeableJump && !input.Jump))
            {
                FSM.SwitchState(typeof(PlayerState_JumpDown));
            }

        }
    }
}
