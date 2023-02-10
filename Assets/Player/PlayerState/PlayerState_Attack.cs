using System;
using System.Collections;
using System.Collections.Generic;
using AnyPortrait;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Attack : PlayerState
    {
        private float AttackEndTime;
        private bool IsAttackEnd = false;
        private bool HasAttack1;
        private bool HasAttack2;
        private bool HasAttack3;
        private bool HasAttack4;
        private bool onAwake = false;
        private apAnimPlayData GAttack1Data;
        private apAnimPlayData GAttack2Data;
        private apAnimPlayData GAttack3Data;
        private apAnimPlayData GAttack4Data;
        private const string Attack1 = "Attack1";
        private const string Attack2 = "Attack2";
        private const string GAttack1 = "GAttack1";
        private const string GAttack2 = "GAttack2";
        private const string GAttack3 = "GAttack3";
        private const string GAttack4 = "GAttack4";

        public override void Enter()
        {
            base.Enter();
            IsAttackEnd = false;

            if (!onAwake)
            {
                //获取所有攻击的PlayData
                onAwake = true;
                GAttack1Data = animManager.GetApAnimPlayData(AnimName.GAttack1);
                GAttack2Data = animManager.GetApAnimPlayData(AnimName.GAttack2);
                GAttack3Data = animManager.GetApAnimPlayData(AnimName.GAttack3);
                GAttack4Data = animManager.GetApAnimPlayData(AnimName.GAttack4);
            }

            if (!HasAttack1)
            {
                HasAttack1 = true;
                animManager.Play(AnimName.GAttack1);
                animManager.effectAnim.Play(GAttack1);
            }
            else if (!HasAttack2)
            {
                HasAttack2 = true;
                animManager.Play(AnimName.GAttack2);
                animManager.effectAnim.Play(GAttack2);
            }
            else if (!HasAttack3)
            {
                HasAttack3 = true;
                animManager.Play(AnimName.GAttack3);
                animManager.effectAnim.Play(GAttack3);
            }
            else if (!HasAttack4)
            {
                HasAttack4 = true;
                animManager.Play(AnimName.GAttack4);
                animManager.effectAnim.Play(GAttack4);
            }

            ctler.Attack(input.AxesX);
            //停顿一下，然后冲出
            //effect.AttackEffect();
        }

        protected override void OnHurtStateSwitchFront()
        {
            ResetAttackState();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //Debug.Log($"{ctler.PlayerVectoryX}");
            // if (!animManager.IsAttacking)
            // {
            //     ctler.SetScale(input.AxesX);
            // }
            if (StateDuration < 0.1f)
            {
                var axesX = Math.Sign(input.AxesX);
                if (axesX != 0 && axesX != ctler.PlayerFace)
                {
                    ctler.SetScale(input.AxesX);
                    ctler.Attack(input.AxesX);
                }
            }

            if (input.RollFrame.Value && propertyController.CanRoll)
            {
                ResetAttackState();
                FSM.SwitchState(typeof(PlayerState_Roll));
            }
            else if (input.JumpFrame.Value && ctler.Grounded)
            {
                ResetAttackState();
                if (ctler.GroundedOneWay && input.AxesY < 0)
                {
                    ctler.OneWayDownFall(null);
                    FSM.SwitchState(typeof(PlayerState_JumpDown));
                }
                else
                {
                    input.JumpFrame.TrueWhenGrounded = true;
                    FSM.SwitchState(typeof(PlayerState_JumpUp));
                }
            }
            else if (ctler.CanUpAttack && input.AttackFrame.Value && ctler.Grounded && input.AxesY > 0.3f)
            {
                ResetAttackState();
                FSM.SwitchState(typeof(PlayerState_UpAttack));
            }
            else if (ctler.CanDownAttack && input.AttackFrame.Value && !ctler.Grounded && input.AxesY < -0.3f)
            {
                ResetAttackState();
                FSM.SwitchState(typeof(PlayerState_DownAttack));
            }
            else if (!IsAttackEnd &&
                    (
                       !HasAttack2 && GAttack1Data.PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended
                    || !HasAttack3 && GAttack2Data.PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended
                    || !HasAttack4 && GAttack3Data.PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended
                    || HasAttack4 && GAttack4Data.PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended
                    )
                    )
            {
                IsAttackEnd = true;
                AttackEndTime = Time.time;
            }
            else if (IsAttackEnd)
            {
                if (Time.time - AttackEndTime > ctler.AttackPostDelay)
                {
                    IsAttackEnd = false;

                    ResetAttackState();

                    if (!ctler.Grounded)
                        FSM.SwitchState(typeof(PlayerState_JumpDown));
                    else if (input.Move)
                        FSM.SwitchState(typeof(PlayerState_Move));
                    else
                        FSM.SwitchState(typeof(PlayerState_Idle));
                }
                else
                {
                    if (input.AttackFrame.Value || input.AttackFrame.IntervalWithLastTrue <= ctler.AttackBufferTime)
                    {
                        IsAttackEnd = false;
                        if (HasAttack4)
                        {
                            ResetAttackState();
                        }
                        FSM.SwitchState(typeof(PlayerState_Attack));
                    }
                }
            }
            // else if (HasAttack1 && animManager.IsAnimEnded(AnimName.GAttack1)
            //         || !HasAttack1 && animManager.IsAnimEnded(AnimName.Attack2))
            // {
            //     if (input.AttackFrame.Value || input.AttackFrame.IntervalWithLastTrue <= ctler.AttackBufferTime)
            //         FSM.SwitchState(typeof(PlayerState_Attack));
            //     else if (!ctler.Grounded)
            //         FSM.SwitchState(typeof(PlayerState_JumpDown));
            //     else if (input.Move)
            //         FSM.SwitchState(typeof(PlayerState_Move));
            //     else
            //         FSM.SwitchState(typeof(PlayerState_Idle));
            // }

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
            ctler.AttackEnd();

        }

        private void ResetAttackState()
        {
            HasAttack1 = false;
            HasAttack2 = false;
            HasAttack3 = false;
            HasAttack4 = false;
        }
    }
}
