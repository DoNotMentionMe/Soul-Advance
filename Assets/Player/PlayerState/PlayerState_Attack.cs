using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Attack : PlayerState
    {
        private float animSwitchCD = 1;
        private float AttackTime;
        private bool HasAttack1;

        public override void Enter()
        {
            base.Enter();
            AttackTime = Time.time;

            if (!HasAttack1 || Time.time - AttackTime >= animSwitchCD)
            {
                HasAttack1 = true;
                animManager.Play(AnimName.Attack1);
            }
            else if (HasAttack1 && Time.time - AttackTime <= animSwitchCD)
            {
                HasAttack1 = false;
                animManager.Play(AnimName.Attack2);
            }
            ctler.Attack();
            //停顿一下，然后冲出
            //effect.AttackEffect();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //Debug.Log($"{ctler.PlayerVectoryX}");
            if (!animManager.IsAttacking)
            {
                ctler.SetScale(input.AxesX);
            }

            if (input.RollFrame.Value)
            {
                FSM.SwitchState(typeof(PlayerState_Roll));
            }
            else if (input.JumpFrame.Value && ctler.Grounded)
            {
                FSM.SwitchState(typeof(PlayerState_JumpUp));
            }
            else if (HasAttack1 && animManager.IsAnimEnded(AnimName.Attack1)
                    || !HasAttack1 && animManager.IsAnimEnded(AnimName.Attack2))
            {
                if (input.AttackFrame.Value || input.AttackFrame.IntervalWithLastTrue <= ctler.AttackBufferTime)
                    FSM.SwitchState(typeof(PlayerState_Attack));
                else if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }

        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();

            ctler.MoveWhenAttack(input.AxesX);
        }

        public override void Exit()
        {
            base.Exit();
            ctler.攻击碰撞体.SetCollEnable(false);
            //effect.AttackEndEffect();
        }
    }
}
