using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_HasWallClimbed : PlayerState
    {
        private float endTimer;
        private AnyPortrait.apAnimPlayData HasWallClimbed;

        public override void Enter()
        {
            base.Enter();

            endTimer = 0;
            HasWallClimbed = apPortrait.CrossFade("HasWallClimbed", 0f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (input.JumpFrame.Value
                || input.JumpFrame.IntervalWithLastTrue <= ctler.ClimbUpJumpBufferTime)
            {
                FSM.SwitchState(typeof(PlayerState_JumpUp));
            }
            // else if (!ctler.Grounded)
            // {
            //     FSM.SwitchState(typeof(PlayerState_JumpDown));
            // }
        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();

            ctler.MoveWhenClimbUp(input.AxesX);

            endTimer += Time.fixedDeltaTime;
            //if (endTimer >= 0.5f)//HasWallClimbed动画长度
            if (HasWallClimbed.PlaybackStatus == AnyPortrait.apAnimPlayData.AnimationPlaybackStatus.Ended)
            {
                if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }
    }
}
