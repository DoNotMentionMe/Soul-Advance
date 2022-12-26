using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Adv
{
    public class PlayerController : MonoBehaviour
    {
        #region 共有属性

        public float PlayerFace => mTransform.localScale.x;
        public float PlayerVectoryX => mRigidbody.velocity.x;
        public float AttackPostDelay => attackPostDelay;
        public float AttackBufferTime => attackBufferTime;
        public float RollBufferTime => rollBufferTime;
        public float JumpBufferTime => jumpBufferTime;
        public float LeaveGroundJumpBufferTime => leaveGroundJumpBufferTime;
        public float ClimbUpJumpBufferTime => climbUpJumpBufferTime;
        public float WallJumpBufferTimeWithnOnAir => wallJumpBufferTimeWithOnAir;
        public float WallJumpBufferTimeWithWallSlide => wallJumpBufferTimeWithWallSlide;
        public bool ChangeableJump => changeableJump;
        public bool JumpDown => mRigidbody.velocity.y <= 0;
        public bool Grounded => GroundCheck.IsTriggered || OneWayGroundCheck.IsTriggered;
        public bool GroundedOneWay => OneWayGroundCheck.IsTriggered;
        public bool canWallClimb_Font => wallFunction && !WallClimbCheck_Font.IsTriggered && WallSlideCheck_Font.IsTriggered;
        public bool canWallClimb_Back => wallFunction && !WallClimbCheck_Back.IsTriggered && WallSlideCheck_Back.IsTriggered;
        public bool WallSlided_Font => wallFunction && WallSlideCheck_Font.IsTriggered;
        public bool WallSlided_Back => wallFunction && WallSlideCheck_Back.IsTriggered;
        public bool canOneWayClimb => wallFunction && OneWayCheck.IsTriggered && !OneWayClimbCheck.IsTriggered;
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
        [SerializeField] float attackPostDelay;
        [SerializeField] float attackBufferTime;
        // [SerializeField] float attackDashTime;
        [SerializeField] float AttackDeceleration;
        [Header("玩家物理属性：移动，翻滚")]
        [SerializeField] float MoveSpeed;
        [SerializeField] float MoveAcceleration;
        [SerializeField] float Movedeceleration;
        [SerializeField] float RollStartSpeed;
        [SerializeField] float RollDeceleration;
        [SerializeField] float RollExtraAcceleration;
        [SerializeField] float RollExtraDeceleration;
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
        [Header("玩家物理属性：单向平台")]
        [SerializeField] float DownFallOffsetY = 0.2f;
        [SerializeField] float DownFallCollSetTrueDelay = 0.3f;
        [Header("检测器")]
        [SerializeField] Trigger2D GroundCheck;
        [SerializeField] Trigger2D WallClimbCheck_Font;
        [SerializeField] Trigger2D WallClimbCheck_Back;
        [SerializeField] Trigger2D WallSlideCheck_Font;
        [SerializeField] Trigger2D WallSlideCheck_Back;
        [SerializeField] Trigger2D OneWayGroundCheck;
        [SerializeField] Trigger2D OneWayClimbCheck;
        [SerializeField] Trigger2D OneWayCheck;
        [SerializeField] Trigger2D m攻击碰撞体;
        [SerializeField] PlayerDownBody 下半体;
        [Header("组件")]
        [SerializeField] PlayerFSM playerFSM;

        #region 私有变量声明和获取、周期函数

        private Rigidbody2D mRigidbody;
        private Transform mTransform;
        private PlayerEffectPerformance effect;
        private Vector2 WallClimbPos;
        private Vector2 EndWallClimbPos;
        private Vector2 attackDirection;
        private Vector2 OnEnablePos;
        private bool HasGetEnablePos;
        private bool IsFixToWallClimbPos;
        private bool FullControlVelocitying = false;
        private float speedRatio = 1;//用来控制攻击时移动减速度的比例
        private enum SetCoord { X, Y }
        private List<float> AttackHittedEffectList = new List<float>();//用来记录执行间隔和执行特效次数
        private Coroutine AttackHittedEffectCorotine;//攻击命中顺序执行协程
        private WaitForSeconds WaitForIntervalOfHittedEffect;//攻击命中执行间隔
        private WaitForSeconds waitForFixedDeltatime;

        private void Awake()
        {
            mRigidbody = GetComponent<Rigidbody2D>();
            mTransform = transform;
            effect = GetComponent<PlayerEffectPerformance>();
            waitForFixedDeltatime = new WaitForSeconds(Time.fixedDeltaTime);
        }

        private void OnEnable()
        {
            if (HasGetEnablePos)
            {
                mTransform.position = OnEnablePos;
            }

        }

        private void OnDestroy()
        {
            mTransform = null;
            mRigidbody = null;
            effect = null;
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
        /// 使用LDtk生成玩家无法获取初始生成位置，通过获取第一次离开Idle的位置来确定
        /// </summary>
        public void GetOnEnablePos()
        {
            HasGetEnablePos = true;
            OnEnablePos = mTransform.position;
        }

        /// <summary>
        /// 用于顿帧使用，该函数将使玩家速度按ratio比例降低ControlTime时间后恢复原速
        /// 同时调用顺序执行
        /// </summary>
        public void FullControlVelocity(float ratio, float ControlTime)
        {
            AttackHittedEffectList.Add(ControlTime);
            if (AttackHittedEffectCorotine == null)
            {
                AttackHittedEffectCorotine = StartCoroutine(ExecuteAttackHittedEvent(ratio));
            }

        }

        public void Move(float AxesX)
        {
            Move(AxesX, MoveSpeed);
        }

        public void Attack(float AxesX)
        {
            if (AxesX == 0)
                SetVelocity(SetCoord.X, SetScale(AxesX) * attackMoveSpeed);
            else
            {
                SetVelocity(SetCoord.X, SetScale(AxesX) * (attackMoveSpeed + attackExtraMoveSpeed));
            }

            if (mRigidbody.velocity.y < 0.1f)
                SetVelocity(SetCoord.Y, 0);

        }

        public void MoveWhenAttack(float AxesX)
        {
            var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, 0, AttackDeceleration * speedRatio * Time.fixedDeltaTime);
            SetVelocity(SetCoord.X, VelocityX);
        }

        public void RollStart(float AxesX)
        {
            SetVelocity(SetCoord.X, SetScale(AxesX) * RollStartSpeed);
        }

        public void Rolling(float AxesX)
        {
            float acceleration = 0;
            if (mTransform.localScale.x * AxesX > 0)
                acceleration = RollDeceleration - RollExtraAcceleration;
            else if (mTransform.localScale.x * AxesX < 0)
                acceleration = RollDeceleration + RollExtraDeceleration;
            else
                acceleration = RollDeceleration;
            var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, 0, acceleration * Time.fixedDeltaTime);
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
            IsFixToWallClimbPos = true;
#if UNITY_EDITOR
            if (DebugWallClimbPos)
            {
                Debug.Log("WallClimbPos: " + WallClimbPos);
                Debug.Log("EndWallClimbPos" + EndWallClimbPos);
            }
#endif
        }

        public void OneWayClimb()
        {
            mRigidbody.velocity = Vector2.zero;

            WallClimbPos = new Vector2(mTransform.position.x,
                                        Mathf.Floor(WallSlideCheck_Font.Pos.y - 0.05f) + WallClimbYOffset1);
            EndWallClimbPos = new Vector2(mTransform.position.x + mTransform.localScale.x * WallClimbXOffset2,
                                        Mathf.Floor(WallSlideCheck_Font.Pos.y - 0.05f) + WallClimbYOffset2);
            IsFixToWallClimbPos = true;
#if UNITY_EDITOR
            if (DebugWallClimbPos)
            {
                Debug.Log("WallClimbPos: " + WallClimbPos);
                Debug.Log("EndWallClimbPos" + EndWallClimbPos);
            }
#endif
        }

        public void OneWayDownFall(UnityAction onCompleted)
        {
            下半体.SetCollEnable(false);
            OneWayClimbCheck.SetCollEnable(false);
            OneWayCheck.SetCollEnable(false);
            DOVirtual.DelayedCall(DownFallCollSetTrueDelay, () =>
            {
                下半体.SetCollEnable(true);
                OneWayClimbCheck.SetCollEnable(true);
                OneWayCheck.SetCollEnable(true);
                onCompleted?.Invoke();
            }, false);
            mTransform.position -= Vector3.up * DownFallOffsetY;
        }

        public void FixPosToWallClimbPos()
        {
            if (IsFixToWallClimbPos)
                mTransform.position = WallClimbPos;
        }

        public void EndWallClimb()
        {
            IsFixToWallClimbPos = false;
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
            mRigidbody.velocity += wallJumpSpeed;
        }

        public void WallLeave()
        {
            var wallLeaveSpeed = WallLeaveSpeed;
            wallLeaveSpeed.x *= mTransform.localScale.x;
            mRigidbody.velocity = wallLeaveSpeed;
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

                var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, direction * Speed, MoveAcceleration * Time.fixedDeltaTime);
                SetVelocity(SetCoord.X, VelocityX);
            }
            else
            {
                var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, 0, Movedeceleration * Time.fixedDeltaTime);
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
                velocity.x = value;
            else
                velocity.y = value;
            mRigidbody.velocity = velocity;
        }

        /// <summary>
        /// 根据输入方向设置玩家面向
        /// </summary>
        /// <param name="direction">需要玩家朝向的方向</param>
        /// <returns>返回当前玩家朝向</returns>
        public float SetScale(float direction)
        {
            if (direction == 0) return mTransform.localScale.x;
            if (mTransform.localScale.x * direction < 0)
            {
                mTransform.localScale *= Vector2.left + Vector2.up;
            }
            return mTransform.localScale.x;
        }

        /// <summary>
        /// 顺序执行减速效果协程
        /// </summary>
        IEnumerator ExecuteAttackHittedEvent(float ratio)
        {
            while (AttackHittedEffectList.Count > 0)
            {
                var controltime = AttackHittedEffectList[0];
                AttackHittedEffectList.RemoveAt(0);
                yield return StartCoroutine(AttackHittedEvent(ratio, controltime));
            }
            AttackHittedEffectCorotine = null;
        }
        /// <summary>
        /// 减速效果协程
        /// </summary>
        IEnumerator AttackHittedEvent(float ratio, float ControlTime)
        {
            FullControlVelocitying = true;
            speedRatio = ratio;
            var velocity = mRigidbody.velocity;
            mRigidbody.velocity *= ratio;
            DOVirtual.DelayedCall(ControlTime, () =>
            {
                if (playerFSM.animManager.IsAttacking)
                    mRigidbody.velocity = velocity;
                speedRatio = 1;
                FullControlVelocitying = false;
            });
            yield return new WaitForSeconds(ControlTime + playerFSM.effect.IntervalOfHittedEffect);
        }
    }
}