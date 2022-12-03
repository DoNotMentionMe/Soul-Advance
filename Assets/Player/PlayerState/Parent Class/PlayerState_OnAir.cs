using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_OnAir : PlayerState
    {
        protected bool BtnJumpInThisState;

        public override void Enter()
        {
            base.Enter();
            BtnJumpInThisState = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (input.JumpFrame.Value && StateDuration > 0.1f) BtnJumpInThisState = true;



        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();



            ctler.Move(input.AxesX);
        }
    }
}
