using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_WallSlide : PlayerState
    {
        private bool ReadyWallLeave;
        private IState JumpUpStateObj;
        private float LeaveWallTimer;

        public override void Enter()
        {
            base.Enter();
            //anim.Play("WallSlide");
            apPortrait.CrossFade("WallSlide", 0.1f);
            LeaveWallTimer = 0f;
            ReadyWallLeave = false;
            if (ctler.WallSlided_Font)
                ctler.FlipPlayer();
            if (JumpUpStateObj == null)
                JumpUpStateObj = FSM.stateTable[typeof(PlayerState_JumpUp)];
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //如果上次按Jump键不在地面就WallJump
            if (input.JumpFrame.Value || (!input.JumpFrame.TrueWhenGrounded && input.JumpFrame.IntervalWithLastTrue <= ctler.WallJumpBufferTimeWithnOnAir))
            {
                FSM.SwitchState(typeof(PlayerState_WallJump));
            }
            // else if (ctler.canWallClimb)
            // {
            //     FSM.SwitchState(typeof(PlayerState_WallClimb));
            // }
            //移动出墙壁，开始判定是否WallJump
            else if (!ReadyWallLeave && input.AxesX * ctler.PlayerFace > 0)
            {
                ctler.WallLeave();
                ReadyWallLeave = true;//WallLeave
            }
            //自然滑出墙壁，就转JumpDown，移动出墙壁，就不判定
            else if (!ReadyWallLeave && StateDuration > 0.1f && !ctler.WallSlided_Back)
            {
                FSM.SwitchState(typeof(PlayerState_JumpDown));
            }
            //落地
            else if (ctler.Grounded)
            {
                if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }


            //移出墙壁，判定是否WallJump，其他逻辑应该写在上面
            if (!ReadyWallLeave) return;//WallLeave To WallJump

            LeaveWallTimer += Time.deltaTime;

            if (input.Jump && LeaveWallTimer <= ctler.WallJumpBufferTimeWithWallSlide)//缓冲时间内按跳跃，判定为WallJump
            {
                Debug.Log("111");
                FSM.SwitchState(typeof(PlayerState_WallJump));
            }
            else if (LeaveWallTimer > ctler.WallJumpBufferTimeWithWallSlide)
            {
                FSM.SwitchState(typeof(PlayerState_JumpDown));
            }

        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();

            if (!ReadyWallLeave)
                ctler.WallSlide();

            if (ReadyWallLeave)
            {
                ctler.Move(input.AxesX);
            }
        }

        public override void Exit()
        {
            base.Exit();
            ReadyWallLeave = false;
        }

        // private void FlipAnim()
        // {
        //     var localScale = anim.transform.localScale;
        //     localScale.x *= -1;
        //     anim.transform.localScale = localScale;
        // }
    }
}
