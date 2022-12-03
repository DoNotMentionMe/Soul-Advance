using System;
using System.Collections;
using System.Collections.Generic;
using AnyPortrait;
using UnityEngine;

namespace Adv
{
    public class PlayerFSM : StateMachine
    {
        //public static GameObject player;

        public PlayerState lastState;

        private PlayerController ctler;
        private PlayerInput input;
        [SerializeField] apPortrait apPortrait;


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

            ctler = GetComponent<PlayerController>();
            input = GetComponent<PlayerInput>();

            Register(new PlayerState_Idle());
            Register(new PlayerState_Move());
            Register(new PlayerState_JumpUp());
            Register(new PlayerState_JumpDown());
            Register(new PlayerState_WallSlide());
            Register(new PlayerState_WallJump());
            Register(new PlayerState_WallClimb());
            Register(new PlayerState_HasWallClimbed());
            Register(new PlayerState_Roll());
        }

        private void OnEnable()
        {
            SwitchOn(typeof(PlayerState_Idle));
        }

        private void OnDestroy()
        {
            stateTable.Clear();
            lastState = null;
        }

        public void Register(PlayerState newState)
        {
            newState.Initialize(
                ctler,
                input,
                apPortrait,
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
            //GUILayout.Label(currentState.GetType().ToString());
            //GUILayout.Label(playerInput.axesX.ToString());
#endif
        }
    }
}