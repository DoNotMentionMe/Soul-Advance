using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_UpAttack : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            ctler.CanotHitBack = true;
            animManager.Play(AnimName.UpAttack);
            animManager.effectAnim.Play("UpAttack");
            ctler.UpAttackStart();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (input.RollFrame.Value && propertyController.CanRoll)
            {
                FSM.SwitchState(typeof(PlayerState_Roll));
            }
            else if (ctler.IsUpAttackEnd)//上升速度很快，这里有一点误差都会差距很大，需要找到解决方法
            {
                ctler.UpAttackEnd();
                if (!ctler.Grounded)
                    FSM.SwitchState(typeof(PlayerState_JumpDown));
                else if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }

        public override void Exit()
        {
            base.Exit();
            ctler.CanotHitBack = false;
            ctler.攻击碰撞体.SetCollEnable(false);
            ctler.UpAttackEnd();
            animManager.effectAnim.Play("Idle");
        }
    }
}
