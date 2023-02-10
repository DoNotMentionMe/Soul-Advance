using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_DownAttack : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            ctler.CanotHitBack = true;
            animManager.Play(AnimName.DownAttack);
            animManager.effectAnim.Play("DownAttack");
            ctler.DownAttackStart();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (input.RollFrame.Value && propertyController.CanRoll)
            {
                FSM.SwitchState(typeof(PlayerState_Roll));
            }
            //上升速度很快，这里有一点误差都会差距很大，需要找到解决方法
            else if (StateDuration >= ctler.DownAttackTime || (StateDuration >= ctler.DownAttackTime / 3 && ctler.Grounded))
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
            ctler.DownAttackEnd();
            animManager.effectAnim.Play("Idle");
        }


    }
}
