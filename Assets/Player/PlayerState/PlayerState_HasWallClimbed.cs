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

            ctler.EndWallClimb();
            animManager.Play(AnimName.HasWallClimbed);
            effect.Release落地灰尘();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // else 
            if (animManager.IsAnimEnded(AnimName.HasWallClimbed))
            {
                // if (input.JumpFrame.IntervalWithLastTrue <= ctler.ClimbUpJumpBufferTime)
                // {
                //     FSM.SwitchState(typeof(PlayerState_JumpUp));
                // }
                // else 
                if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }
            else if (input.JumpFrame.Value
                  || input.JumpFrame.IntervalWithLastTrue <= ctler.ClimbUpJumpBufferTime
                )
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

            //ctler.MoveWhenClimbUp(input.AxesX);
            ctler.Move(input.AxesX);
            // if (!ctler.Grounded)
            // {
            //     FSM.SwitchState(typeof(PlayerState_JumpDown));
            // }

        }
    }
}
