using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Attack : PlayerState
    {
        public override void Enter()
        {
            base.Enter();

            animManager.Play(AnimName.Attack);
            //停顿一下，然后冲出
            ctler.Attack();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!ctler.IsAttacking)
            {
                FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }
    }
}
