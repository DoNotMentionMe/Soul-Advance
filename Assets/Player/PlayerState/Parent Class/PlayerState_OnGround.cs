using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_OnGround : PlayerState
    {
        // public override void LogicUpdate()
        // {
        //     if (input.JumpFrame.Value
        //         || input.JumpFrame.IntervalWithLastTrue <= ctler.ClimbUpJumpBufferTime)
        //     {
        //         FSM.SwitchState(typeof(PlayerState_JumpUp));
        //     }
        //     else if (!ctler.Grounded)
        //     {
        //         if (input.JumpFrame.IntervalWithLastTrue <= ctler.LeaveGroundJumpBufferTime)
        //             FSM.SwitchState(typeof(PlayerState_JumpUp));
        //         else
        //             FSM.SwitchState(typeof(PlayerState_JumpDown));
        //     }
        //     else if (input.RollFrame.Value)
        //     {
        //         FSM.SwitchState(typeof(PlayerState_Roll));
        //     }
        // }

        // public override void PhysicUpdate()
        // {
        //     base.PhysicUpdate();
        //     ctler.Move(input.AxesX);
        // }
    }
}
