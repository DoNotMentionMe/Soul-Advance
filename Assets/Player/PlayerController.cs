using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Adv
{
    public class PlayerController : MonoBehaviour
    {
        #region 共有属性

        public float PlayerFace => mTransform.localScale.x;
        public float PlayerVectoryX => mRigidbody.velocity.x;
        public float AttackBufferTime => attackBufferTime;
        public float RollBufferTime => rollBufferTime;
        public float JumpBufferTime => jumpBufferTime;
        public float LeaveGroundJumpBufferTime => leaveGroundJumpBufferTime;
        public float ClimbUpJumpBufferTime => climbUpJumpBufferTime;
        public float WallJumpBufferTimeWithnOnAir => wallJumpBufferTimeWithOnAir;
        public float WallJumpBufferTimeWithWallSlide => wallJumpBufferTimeWithWallSlide;
        public bool ChangeableJump => changeableJump;
        public bool JumpDown => mRigidbody.velocity.y <= 0;
        public bool Grounded => GroundCheck.IsTriggered;
        public bool canWallClimb_Font => wallFunction && !WallClimbCheck_Font.IsTriggered;
        public bool canWallClimb_Back => wallFunction && !WallClimbCheck_Back.IsTriggered;
        public bool WallSlided_Font => wallFunction && WallSlideCheck_Font.IsTriggered;
        public bool WallSlided_Back => wallFunction && WallSlideCheck_Back.IsTriggered;
        public Trigger2D 攻击碰撞体 => m攻击碰撞体;

        #endregion

        [Header("能力开启")]
        [SerializeField] bool wallFunction;
        [SerializeField] bool changeableJump;
        [Space]
        [Header("基本物理属性")]
        [SerializeField] float Gravity;
        [SerializeField] float MaxFallSpeed;
        [Header("玩家物理属性：攻击")]
        [SerializeField] float attackMoveSpeed;
        [SerializeField] float attackExtraMoveSpeed;
        [SerializeField] float attackBufferTime;
        // [SerializeField] float attackDashTime;
        [SerializeField] float AttackDeceleration;
        [Header("玩家物理属性：移动，翻滚")]
        [SerializeField] float MoveSpeed;
        [SerializeField] float MoveAcceleration;
        [SerializeField] float Movedeceleration;
        [SerializeField] float RollSpeed;
        [SerializeField] float RollAcceleration;
        [SerializeField] float rollBufferTime;
        [Header("玩家物理属性：普通跳跃")]
        [SerializeField] float JumpForce;
        [SerializeField, Range(0, 1)] float ScaleChangeableJump;
        [SerializeField] float jumpBufferTime;//OnAir时缓冲时间内按跳跃键判定为Jump或WallJump
        [SerializeField] float leaveGroundJumpBufferTime;
        [SerializeField] float climbUpJumpBufferTime;
        [Header("玩家物理属性：滑墙、跳墙、爬墙")]
        [SerializeField] bool DebugWallClimbPos;
        [SerializeField] Vector2 WallJumpSpeed;
        [SerializeField] Vector2 WallLeaveSpeed;
        [SerializeField] float WallSlideSpeed;
        [SerializeField] float wallJumpBufferTimeWithOnAir;//空中时缓冲时间内按跳跃键判定为WallJump
        [SerializeField] float wallJumpBufferTimeWithWallSlide;//WallLeave时缓冲时间内按跳跃键判定为WallJump
        [SerializeField] float decelerationWhenWallJumpStart;//wallJump时如果还是往同一边移动，设置为该速度
        [SerializeField] float ClimbUpMoveSpeed;
        [SerializeField] float WallClimbXOffset1;
        [SerializeField] float WallClimbYOffset1;
        [SerializeField] float WallClimbXOffset2;
        [SerializeField] float WallClimbYOffset2;
        [Header("检测器")]
        [SerializeField] Trigger2D GroundCheck;
        [SerializeField] Trigger2D WallClimbCheck_Font;
        [SerializeField] Trigger2D WallClimbCheck_Back;
        [SerializeField] Trigger2D WallSlideCheck_Font;
        [SerializeField] Trigger2D WallSlideCheck_Back;
        [SerializeField] Trigger2D m攻击碰撞体;
        [Header("组件")]
        [SerializeField] PlayerAnimManager animManager;

        #region 私有变量声明和获取、周期函数

        private Rigidbody2D mRigidbody;
        private Transform mTransform;
        private PlayerEffectPerformance effect;
        private Vector2 WallClimbPos;
        private Vector2 EndWallClimbPos;
        private Vector2 attackDirection;
        private bool IsStop;
        private bool FullControlVelocitying = false;
        private float speedRatio = 1;//用来控制所有速度的比例
        private enum SetCoord { X, Y }
        private WaitForSeconds waitForFixedDeltatime;

        private void Awake()
        {
            mRigidbody = GetComponent<Rigidbody2D>();
            mTransform = transform;
            effect = GetComponent<PlayerEffectPerformance>();
            waitForFixedDeltatime = new WaitForSeconds(Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            mTransform = null;
            mRigidbody = null;
            effect = null;
        }

        private void Update()
        {
            if (IsStop)
                mTransform.position = WallClimbPos;
        }

        private void FixedUpdate()
        {
            //重力计算
            if (FullControlVelocitying) return;
            if (mRigidbody.velocity.y > -MaxFallSpeed)
            {
                mRigidbody.velocity += Vector2.down * Gravity * Time.fixedDeltaTime;
            }
            else
            {
                SetVelocity(SetCoord.Y, -MaxFallSpeed);
            }
        }


        #endregion

        #region 对外API

        /// <summary>
        /// 用于顿帧使用，该函数将使玩家速度按ratio比例降低ControlTime时间后恢复原速
        /// </summary>
        public void FullControlVelocity(float ratio, float ControlTime)
        {
            if (FullControlVelocitying) return;

            FullControlVelocitying = true;
            speedRatio = ratio;
            mRigidbody.velocity *= ratio;
            DOVirtual.DelayedCall(ControlTime, () =>
            {
                mRigidbody.velocity /= ratio;
                speedRatio = 1;
                FullControlVelocitying = false;
            });
        }

        public void Attack()
        {
            float direction = mTransform.localScale.x;
            SetVelocity(SetCoord.X, direction * attackMoveSpeed);
        }

        public void Move(float AxesX)
        {
            Move(AxesX, MoveSpeed);
        }

        public void MoveWhenAttack(float AxesX)
        {
            var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, 0, AttackDeceleration * speedRatio * Time.fixedDeltaTime);
            SetVelocity(SetCoord.X, VelocityX);
        }

        public void Rolling(float AxesX)
        {
            float Speed;
            if (mTransform.localScale.x * AxesX > 0)
                Speed = RollAcceleration + RollSpeed;
            else if (mTransform.localScale.x * AxesX < 0)
                Speed = RollSpeed - RollAcceleration;
            else
                Speed = RollSpeed;
            var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, mTransform.localScale.x * Speed, MoveAcceleration * speedRatio * Time.fixedDeltaTime);
            SetVelocity(SetCoord.X, VelocityX);
        }

        public void Jump()
        {
            SetVelocity(SetCoord.Y, JumpForce);
        }

        public void WallClimb()
        {
            mRigidbody.velocity = Vector2.zero;
            if (mTransform.localScale.x > 0)//向右
            {
                WallClimbPos = new Vector2(Mathf.Floor(WallSlideCheck_Font.Pos.x + WallSlideCheck_Font.Length / 2 + 0.05f) - WallClimbXOffset1,
                                            Mathf.Floor(WallSlideCheck_Font.Pos.y - 0.05f) + WallClimbYOffset1);
                EndWallClimbPos = new Vector2(Mathf.Floor(WallSlideCheck_Font.Pos.x + WallSlideCheck_Font.Length / 2 + 0.05f) + WallClimbXOffset2,
                                            Mathf.Floor(WallSlideCheck_Font.Pos.y - 0.05f) + WallClimbYOffset2);
            }
            else
            {
                WallClimbPos = new Vector2(Mathf.Ceil(WallSlideCheck_Font.Pos.x - WallSlideCheck_Font.Length / 2 - 0.05f) + WallClimbXOffset1,
                                            Mathf.Floor(WallSlideCheck_Font.Pos.y - 0.05f) + WallClimbYOffset1);
                EndWallClimbPos = new Vector2(Mathf.Ceil(WallSlideCheck_Font.Pos.x - WallSlideCheck_Font.Length / 2 - 0.05f) - WallClimbXOffset2,
                                            Mathf.Floor(WallSlideCheck_Font.Pos.y - 0.05f) + WallClimbYOffset2);
            }
            IsStop = true;
#if UNITY_EDITOR
            if (DebugWallClimbPos)
            {
                Debug.Log("WallClimbPos: " + WallClimbPos);
                Debug.Log("EndWallClimbPos" + EndWallClimbPos);
            }
#endif
        }
        public void EndWallClimb()
        {
            IsStop = false;
            mTransform.position = EndWallClimbPos;
        }

        public void DecelerationWhenChangeableJump()
        {
            var speed = mRigidbody.velocity.y * ScaleChangeableJump;
            SetVelocity(SetCoord.Y, speed);
        }

        public void WallJump()
        {
            mRigidbody.velocity = Vector2.zero;
            var wallJumpSpeed = WallJumpSpeed;
            wallJumpSpeed.x *= mTransform.localScale.x;
            mRigidbody.velocity += wallJumpSpeed * speedRatio;
        }

        public void WallLeave()
        {
            var wallLeaveSpeed = WallLeaveSpeed;
            wallLeaveSpeed.x *= mTransform.localScale.x;
            mRigidbody.velocity = wallLeaveSpeed * speedRatio;
        }

        public void WallSlide()
        {
            SetVelocity(SetCoord.Y, -WallSlideSpeed);
        }

        public void DecelerationWhenWallJumpStart()
        {
            mRigidbody.velocity += Vector2.down * decelerationWhenWallJumpStart * Time.fixedDeltaTime;
        }

        public void FlipPlayer()
        {
            SetScale(-(int)mTransform.localScale.x);
        }

        #endregion

        private void Move(float AxesX, float Speed)
        {
            float direction = 0;
            if (AxesX != 0)//加速
            {
                direction = Mathf.Sign(AxesX);

                SetScale((int)direction);

                var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, direction * Speed, MoveAcceleration * speedRatio * Time.fixedDeltaTime);
                SetVelocity(SetCoord.X, VelocityX);
            }
            else
            {
                var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, 0, Movedeceleration * speedRatio * Time.fixedDeltaTime);
                SetVelocity(SetCoord.X, VelocityX);
            }
        }



        /// <summary>
        /// 设置单个坐标方向的速度
        /// </summary>
        private void SetVelocity(SetCoord coord, float value)
        {
            var velocity = mRigidbody.velocity;
            if (coord == SetCoord.X)
                velocity.x = value * speedRatio;
            else
                velocity.y = value * speedRatio;
            mRigidbody.velocity = velocity;
        }

        /// <summary>
        /// 根据输入方向设置玩家面向
        /// </summary>
        /// <param name="direction">只能是1，-1</param>
        public void SetScale(float direction)
        {
            if (direction == 0) return;
            if (mTransform.localScale.x * direction < 0)
            {
                mTransform.localScale *= Vector2.left + Vector2.up;
            }
        }
    }
}