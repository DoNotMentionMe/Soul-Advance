using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_HasWallClimbed : PlayerState
    {

        public override void Enter()
        {
            base.Enter();

            animManager.Play(AnimName.HasWallClimbed);
            ctler.EndWallClimb();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // else 
            if (animManager.IsAnimEnded(AnimName.HasWallClimbed))
            {
                if (input.JumpFrame.Value
                || input.JumpFrame.IntervalWithLastTrue <= ctler.ClimbUpJumpBufferTime)
                {
                    FSM.SwitchState(typeof(PlayerState_JumpUp));
                }
                else if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }
            // else if (!ctler.Grounded)
            // {
            //     FSM.SwitchState(typeof(PlayerState_JumpDown));
            // }
        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();

            //ctler.MoveWhenClimbUp(input.AxesX);

        }
    }
}
