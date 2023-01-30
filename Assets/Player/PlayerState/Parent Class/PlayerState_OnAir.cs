using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_OnAir : PlayerState
    {
        //protected bool BtnJumpInThisState;

        public override void Enter()
        {
            base.Enter();
            //BtnJumpInThisState = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //if (input.JumpFrame.Value && StateDuration > 0.1f) BtnJumpInThisState = true;

            if (input.RollFrame.Value && propertyController.CanRoll)
                FSM.SwitchState(typeof(PlayerState_Roll));
            else if (input.AttackFrame.Value)
            {
                FSM.SwitchState(typeof(PlayerState_Attack));
            }


        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();



            ctler.Move(input.AxesX);
        }
    }
}
