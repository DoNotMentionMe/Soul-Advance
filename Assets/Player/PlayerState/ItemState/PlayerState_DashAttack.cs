using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_DashAttack : PlayerState
    {
        private DashAttack dashAttack;
        private float currentDirection;
        private float CTimer;
        private bool IsSetVelocity;

        public override void Enter()
        {
            base.Enter();
            CTimer = 0f;
            IsSetVelocity = false;
            if (dashAttack == null) dashAttack = Resources.Load<DashAttack>("Item/DashAttack");
            animManager.CrossFade(AnimName.DashAttack);
            animManager.effectAnim.Play("DashAttack");
            ctler.CanotHitBack = true;
            ctler.NotInjury(true);
            ctler.StopFullControlVelocity();
            ctler.FullControlVelocitying = true;
            currentDirection = Math.Sign(input.AxesX);
            if (currentDirection == 0)
                currentDirection = ctler.PlayerFace;
            else
                ctler.SetScale(currentDirection);
            ctler.SetVelocity(PlayerController.SetCoord.X, 0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!IsSetVelocity && animManager.IsAnimEnded(AnimName.DashAttack))
            {
                IsSetVelocity = true;
                ctler.SetVelocity(PlayerController.SetCoord.X, currentDirection * dashAttack.DashAttackSpeed);
                ctler.SetVelocity(PlayerController.SetCoord.Y, 0);
            }

            if (CTimer >= dashAttack.DashAttackTime)
            {
                if (ctler.HeadTouchGround)
                {
                    ctler.SetVelocity(PlayerController.SetCoord.X, currentDirection * dashAttack.DashAttackSpeed);
                }
                else if (!ctler.Grounded)
                {
                    FSM.SwitchState(typeof(PlayerState_JumpDown));
                }
                else if (input.Move)
                    FSM.SwitchState(typeof(PlayerState_Move));
                else if (!input.Move)
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            if (IsSetVelocity)
                CTimer += Time.fixedDeltaTime;
        }

        public override void Exit()
        {
            base.Exit();
            ctler.SetVelocity(PlayerController.SetCoord.X, 0);
            ctler.FullControlVelocitying = false;
            ctler.CanotHitBack = false;
            ctler.NotInjury(false);
            ctler.攻击碰撞体.SetCollEnable(false);
            animManager.effectAnim.Play("Idle");
        }
    }
}
