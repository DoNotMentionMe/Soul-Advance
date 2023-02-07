using System;
using System.Collections;
using System.Collections.Generic;
using AnyPortrait;
using UnityEngine;

namespace Adv
{
    public class PlayerFSM : StateMachine
    {
        public static PlayerFSM Player;

        public PlayerState lastState;

        public PlayerController ctler;
        public PlayerInput input;
        public PlayerEffectPerformance effect;
        public apPortrait apPortrait;
        public PlayerAnimManager animManager;
        public PlayerPropertyController propertyController;


        private void Awake()
        {
            // if (player == null)
            // {
            //     player = this.gameObject;
            // }
            // else if (player != this.gameObject)
            // {
            //     Destroy(gameObject);
            // }
            // DontDestroyOnLoad(gameObject);

            Register(new PlayerState_Idle());
            Register(new PlayerState_Move());
            Register(new PlayerState_JumpUp());
            Register(new PlayerState_JumpDown());
            Register(new PlayerState_WallSlide());
            Register(new PlayerState_WallJump());
            Register(new PlayerState_WallClimb());
            Register(new PlayerState_HasWallClimbed());
            Register(new PlayerState_Roll());
            Register(new PlayerState_Attack());
            Register(new PlayerState_Hurt());
            Register(new PlayerState_UpAttack());
            Register(new PlayerState_DownAttack());
            //Register(new PlayerState_StabAttack());
        }

        private void OnEnable()
        {
            Player = this;
            SwitchOn(typeof(PlayerState_Idle));
        }

        private void OnDestroy()
        {
            stateTable.Clear();
            lastState = null;
            input = null;
            ctler = null;
            effect = null;
        }

        public void Register(PlayerState newState)
        {
            newState.Initialize(
                ctler,
                input,
                effect,
                apPortrait,
                animManager,
                propertyController,
                this);
            stateTable.Add(newState.GetType(), newState);
        }

        public override void SwitchState(Type stateKey)
        {
            //Debug.Log(stateKey.Name);
            lastState = (PlayerState)currentState;
            base.SwitchState(stateKey);
        }

        private void OnGUI()
        {
#if UNITY_EDITOR
            GUILayout.Label(currentState.GetType().ToString());
            //GUILayout.Label(playerInput.axesX.ToString());
#endif
        }
    }
}