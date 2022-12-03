using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_WallJump : PlayerState_OnAir
    {
        private bool HasWallJump;
        private float FaceWhenEnterWallJump;
        private IState WallSlideObj;

        public override void Enter()
        {
            HasWallJump = false;
            FaceWhenEnterWallJump = ctler.PlayerFace;
            base.Enter();
            //anim.Play("JumpUp");
            apPortrait.CrossFade("WallJump", 0f);
            if (WallSlideObj == null)
                WallSlideObj = FSM.stateTable[typeof(PlayerState_WallSlide)];
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!HasWallJump)//不知名物理BUG或状态转换问题，导致Enter执行WallJump会失效
            {
                HasWallJump = true;
                ctler.WallJump();
            }

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
            // else if (ctler.WallSlided_Font && BtnJumpInThisState && input.JumpFrame.IntervalWithLastTrue <= ctler.WallJumpBufferTimeWithnOnAir)
            // {
            //     ctler.FlipPlayer();
            //     FSM.SwitchState(typeof(PlayerState_WallJump));
            // }
            //JumpDown or 可变跳
            else if ((ctler.JumpDown || (ctler.ChangeableJump && !input.Jump)) && StateDuration > 0.1f)
            {
                FSM.SwitchState(typeof(PlayerState_JumpDown));
            }
            //没什么意义
            // else if (ctler.Grounded && StateDuration > 0.1f)
            // {
            //     FSM.SwitchState(typeof(PlayerState_Idle));
            // }
            //滑落
            else if ((ctler.WallSlided_Back || ctler.WallSlided_Font) && StateDuration > 0.1f && !input.Jump)//这里可能出BUG
            {
                FSM.SwitchState(typeof(PlayerState_WallSlide));
            }
        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            if (FSM.lastState == WallSlideObj && StateDuration > 0.1f && input.AxesX * FaceWhenEnterWallJump < 0)
            {
                ctler.DecelerationWhenWallJumpStart();
            }
        }
    }
}
