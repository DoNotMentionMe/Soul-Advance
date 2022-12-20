using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState : IState
    {
        protected float StateDuration => Time.time - stateStartTime;

        protected const string Idle = "Idle";
        protected const string RightAttackStart = "RightAttackStart";
        protected const string RightAttack = "RightAttack";
        protected const string RightAttackEnd = "RightAttackEnd";
        protected const string UpAttackStart = "UpAttackStart";
        protected const string UpAttack = "UpAttack";
        protected const string UpAttackEnd = "UpAttackEnd";

        protected PlayerController ctler;
        protected PlayerInput input;
        protected PlayerEffectPerformance effect;
        protected AnyPortrait.apPortrait apPortrait;
        protected PlayerAnimManager animManager;
        protected PlayerFSM FSM;

        protected float stateFixedFrameCount = 0;

        protected float stateStartTime;

        public virtual void Initialize(
                                PlayerController playerController,
                                PlayerInput playerInput,
                                PlayerEffectPerformance effect,
                                AnyPortrait.apPortrait apPortrait,
                                PlayerAnimManager animManager,
                                PlayerFSM FSM)
        {
            this.ctler = playerController;
            this.input = playerInput;
            this.effect = effect;
            this.apPortrait = apPortrait;
            this.animManager = animManager;
            this.FSM = FSM;
        }

        public virtual void Enter()
        {
            stateStartTime = Time.time;
            stateFixedFrameCount = 0;
        }

        public virtual void Exit()
        {
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicUpdate()
        {
            stateFixedFrameCount += 1;//1秒50帧，一帧0.02秒
        }

        private void OnDestroy()
        {
            FSM = null;
        }

        public virtual void OnAwake()
        {

        }

        public PlayerState()
        {
            OnAwake();
        }
    }
}