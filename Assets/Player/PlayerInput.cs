using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Adv
{
    public class PlayerInput : MonoBehaviour,
                                PlayerInputActions.IGameplayActions
    {
        private PlayerInputActions playerInput;

        public Bool JumpFrame = new Bool(false);//只有按下瞬间那一帧为true
        public Bool MoveFrame = new Bool(false);//只有按下瞬间那一帧为true
        public Bool RollFrame = new Bool(false);//只有按下瞬间那一帧为true
        public Bool AttackFrame = new Bool(false);

        public bool Jump { get; set; }//记录按键状态
        public bool Roll { get; set; }
        public bool Attack { get; set; }
        public bool Move => AxesX != 0;

        public float AxesX => axes.x;
        public float AxesY => axes.y;

        private Vector2 axes;


        private void Awake()
        {
            playerInput = new PlayerInputActions();
            playerInput.Gameplay.SetCallbacks(this);

            EnableGameplayInput();
        }

        private void OnDestroy()
        {
            DisableAllInputs();
            JumpFrame = null;
            MoveFrame = null;
            RollFrame = null;
            AttackFrame = null;
        }

        public void EnableGameplayInput() => playerInput.Gameplay.Enable();

        public void DisableAllInputs()
        {
            playerInput.Gameplay.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Jump = true;
                JumpFrame.Value = true;
                StartSetBoolFalse(JumpFrame);
            }
            else if (context.canceled)
            {
                Jump = false;
            }
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                axes = context.ReadValue<Vector2>();
                MoveFrame.Value = true;
                StartSetBoolFalse(MoveFrame);
            }
            else if (context.canceled)
            {
                axes = Vector2.zero;
            }
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Roll = true;
                RollFrame.Value = true;
                StartSetBoolFalse(RollFrame);
            }
            else if (context.canceled)
            {
                Roll = false;
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Attack = true;
                AttackFrame.Value = true;
                StartSetBoolFalse(AttackFrame);
            }
            else if (context.canceled)
            {
                Attack = false;
            }
        }

        private void StartSetBoolFalse(Bool boolObj)
        {
            StartCoroutine(SetBoolFalse(boolObj));
        }

        IEnumerator SetBoolFalse(Bool boolObj)
        {
            yield return null;
            boolObj.Value = false;
        }
    }

    public class Bool
    {
        public float IntervalWithLastTrue => Time.time - setTrueTime;//这个变量只能用来比小于
        public bool TrueWhenGrounded;//修复地上跳起马上WallSlide会直接WallJump的BUG
        public bool Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                if (value == true)
                {
                    setTrueTime = Time.time;
                    TrueWhenGrounded = false;
                }
            }
        }

        private bool value;
        private float setTrueTime = -100f;

        public Bool() { }
        public Bool(bool value)
        {
            this.value = value;
        }
    }
}