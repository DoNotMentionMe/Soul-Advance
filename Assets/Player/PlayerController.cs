using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using NaughtyAttributes;
using System;

namespace Adv
{
    public enum RollType { Roll, DashAttack, }
    public class PlayerController : MonoBehaviour
    {
        #region 共有属性

        public float PlayerFace => transform.localScale.x;
        public float PlayerVectoryX => mRigidbody.velocity.x;
        public float AttackPostDelay => attackPostDelay;
        public float AttackBufferTime => attackBufferTime;
        public float DownAttackTime => downAttackTime;
        public float RollBufferTime => rollBufferTime;
        public float JumpBufferTime => jumpBufferTime;
        public float LeaveGroundJumpBufferTime => leaveGroundJumpBufferTime;
        public float ClimbUpJumpBufferTime => climbUpJumpBufferTime;
        public float WallJumpBufferTimeWithnOnAir => wallJumpBufferTimeWithOnAir;
        public float WallJumpBufferTimeWithWallSlide => wallJumpBufferTimeWithWallSlide;
        public float HurtStateTime => hurtStateTime * HurtStateFactor;
        public bool IsUpAttackEnd => transform.position.y - upAttackStartPosY >= upAttackDistance || mRigidbody.velocity.y < 0;
        public bool ChangeableJump => changeableJump;
        public bool JumpDown => mRigidbody.velocity.y <= 0;
        public bool Grounded => GroundCheck.IsTriggered || OneWayGroundCheck.IsTriggered;
        public bool GroundedOneWay => OneWayGroundCheck.IsTriggered;
        public bool HeadTouchGround => HeadCheck.IsTriggered;
        public bool canWallClimb_Font => wallFunction && !WallClimbCheck_Font.IsTriggered && WallSlideCheck_Font.IsTriggered;
        public bool canWallClimb_Back => wallFunction && !WallClimbCheck_Back.IsTriggered && WallSlideCheck_Back.IsTriggered;
        public bool WallSlided_Font => wallFunction && WallSlideCheck_Font.IsTriggered;
        public bool WallSlided_Back => wallFunction && WallSlideCheck_Back.IsTriggered;
        public bool canOneWayClimb => wallFunction && OneWayCheck.IsTriggered && !OneWayClimbCheck.IsTriggered;
        public Trigger2D 攻击碰撞体 => m攻击碰撞体;

        #endregion

        public float HurtBackFactor { get; set; } = 1;//受伤击退因数
        public float HurtStateFactor { get; set; } = 1;//受伤僵直时间因数
        [HideInInspector] public bool Attacking = false;
        [HideInInspector] public bool CanotHitBack = false;
        public RollType rollType = RollType.Roll;

        #region 序列化变量
        [Foldout("能力开启")][SerializeField] bool wallFunction;
        [Foldout("能力开启")][SerializeField] bool changeableJump;
        [Foldout("能力开启")] public bool CanUpAttack = false;
        [Foldout("能力开启")] public bool CanDownAttack = false;
        [Foldout("基本物理属性")][SerializeField] float Gravity;
        [Foldout("基本物理属性")][SerializeField] float MaxFallSpeed;
        [Foldout("玩家物理属性：攻击")][SerializeField] float attackMoveSpeed;
        [Foldout("玩家物理属性：攻击")][SerializeField] float attackExtraMoveSpeed;
        [Foldout("玩家物理属性：攻击")][SerializeField] float attackPostDelay;//攻击后停顿时间
        [Foldout("玩家物理属性：攻击")][SerializeField] float attackBufferTime;//指令缓存时间
        [Foldout("玩家物理属性：攻击")][SerializeField] float AttackDeceleration;
        [Foldout("玩家物理属性：上挑攻击")][SerializeField] float upAttackSpeed;
        [Foldout("玩家物理属性：上挑攻击")][SerializeField] float upAttackDistance;
        [Foldout("玩家物理属性：下劈攻击")][SerializeField] float downAttackSpeed;
        [Foldout("玩家物理属性：下劈攻击")][SerializeField] float downAttackTime;
        [Foldout("玩家物理属性：移动，翻滚")][SerializeField] float MoveSpeed;
        [Foldout("玩家物理属性：移动，翻滚")][SerializeField] float MoveAcceleration;
        [Foldout("玩家物理属性：移动，翻滚")][SerializeField] float Movedeceleration;
        [Foldout("玩家物理属性：移动，翻滚")][SerializeField] float RollStartSpeed;
        [Foldout("玩家物理属性：移动，翻滚")][SerializeField] float RollHoldSpeed;
        [Foldout("玩家物理属性：移动，翻滚")][SerializeField] float RollDeceleration;
        [Foldout("玩家物理属性：移动，翻滚")][SerializeField] float RollExtraAcceleration;
        [Foldout("玩家物理属性：移动，翻滚")][SerializeField] float RollExtraDeceleration;
        [Foldout("玩家物理属性：移动，翻滚")][SerializeField] float rollBufferTime;
        [Foldout("玩家物理属性：普通跳跃")][SerializeField] float JumpForce;
        [Foldout("玩家物理属性：普通跳跃")][SerializeField, Range(0, 1)] float ScaleChangeableJump;
        [Foldout("玩家物理属性：普通跳跃")][SerializeField] float jumpBufferTime;//OnAir时缓冲时间内按跳跃键判定为Jump或WallJump
        [Foldout("玩家物理属性：普通跳跃")][SerializeField] float leaveGroundJumpBufferTime;
        [Foldout("玩家物理属性：普通跳跃")][SerializeField] float climbUpJumpBufferTime;
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] bool DebugWallClimbPos;
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] Vector2 WallJumpSpeed;
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] Vector2 WallLeaveSpeed;
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] float WallSlideSpeed;
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] float wallJumpBufferTimeWithOnAir;//空中时缓冲时间内按跳跃键判定为WallJump
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] float wallJumpBufferTimeWithWallSlide;//WallLeave时缓冲时间内按跳跃键判定为WallJump
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] float decelerationWhenWallJumpStart;//wallJump时如果还是往同一边移动，设置为该速度
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] float ClimbUpMoveSpeed;
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] float WallClimbXOffset1;
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] float WallClimbYOffset1;
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] float WallClimbXOffset2;
        [Foldout("玩家物理属性：滑墙、跳墙、爬墙")][SerializeField] float WallClimbYOffset2;
        [Foldout("玩家物理属性：单向平台")][SerializeField] float DownFallOffsetY = 0.2f;
        [Foldout("玩家物理属性：单向平台")][SerializeField] float DownFallCollSetTrueDelay = 0.3f;
        [Foldout("玩家物理属性：受伤")][SerializeField] float hurtStateTime;
        [Foldout("玩家物理属性：受伤")][SerializeField] float HurtNotInjuryTime;
        [Foldout("玩家物理属性：受伤")][SerializeField] float HurtHoriontalForce;
        [Foldout("玩家物理属性：受伤")][SerializeField] float HurtJumpForce;
        [Foldout("玩家物理属性：受伤")][SerializeField] GameObjectEventChannel On玩家受伤Event;
        [Foldout("玩家物理属性：受伤")][SerializeField] GameObject Attacker;
        [Foldout("检测器")][SerializeField] Trigger2D GroundCheck;
        [Foldout("检测器")][SerializeField] Trigger2D HeadCheck;
        [Foldout("检测器")][SerializeField] Trigger2D WallClimbCheck_Font;
        [Foldout("检测器")][SerializeField] Trigger2D WallClimbCheck_Back;
        [Foldout("检测器")][SerializeField] Trigger2D WallSlideCheck_Font;
        [Foldout("检测器")][SerializeField] Trigger2D WallSlideCheck_Back;
        [Foldout("检测器")][SerializeField] Trigger2D OneWayGroundCheck;
        [Foldout("检测器")][SerializeField] Trigger2D OneWayClimbCheck;
        [Foldout("检测器")][SerializeField] Trigger2D OneWayCheck;
        [Foldout("检测器")][SerializeField] Trigger2D m攻击碰撞体;
        [Foldout("检测器")][SerializeField] PlayerDownBody 下半体;
        [Foldout("组件")][SerializeField] PlayerFSM playerFSM;
        [Foldout("组件")][SerializeField] Collider2D mColl;
        [Foldout("组件")][SerializeField] PlayerProperty property;

        #endregion

        #region 私有变量声明和获取、周期函数

        private Rigidbody2D mRigidbody;
        private PlayerEffectPerformance effect;
        private Vector2 WallClimbPos;
        private Vector2 EndWallClimbPos;
        private Vector2 attackDirection;
        private Vector2 OnEnablePos;
        private bool HasGetEnablePos;
        private bool IsFixToWallClimbPos;
        [HideInInspector] public bool FullControlVelocitying = false;
        private float speedRatio = 1;//用来控制攻击时移动减速度的比例
        private float currentHittedBackForce;
        private float upAttackStartPosY;
        public enum SetCoord { X, Y }
        private List<float> AttackHittedEffectList = new List<float>();//用来记录执行间隔和执行特效次数
        private Coroutine AttackHittedEffectCorotine;//攻击命中顺序执行协程
        private Coroutine HurtNotInjuryCoroutine;
        private WaitForSecondsRealtime waitForIntervalHittedEffect;
        private WaitForSecondsRealtime waitForAttackHittedFreezeTime;
        private WaitForSecondsRealtime waitForSecondFreezeTime;
        private WaitForSeconds waitForFixedDeltatime;
        private WaitForSeconds waitForHurtNotInjuryTime;

        private void Awake()
        {
            mRigidbody = GetComponent<Rigidbody2D>();
            effect = GetComponent<PlayerEffectPerformance>();
            waitForFixedDeltatime = new WaitForSeconds(Time.fixedDeltaTime);
            waitForHurtNotInjuryTime = new WaitForSeconds(HurtNotInjuryTime);
            On玩家受伤Event.AddListener(HurtHitBack);
        }

        private void Start()
        {
            waitForIntervalHittedEffect = new WaitForSecondsRealtime(playerFSM.effect.IntervalOfHittedEffect);
            waitForAttackHittedFreezeTime = new WaitForSecondsRealtime(playerFSM.effect.AttackHittedFreezeTime);
            waitForSecondFreezeTime = new WaitForSecondsRealtime(playerFSM.effect.SecondFreezeTime);
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            AttackHittedEffectCorotine = null;
            HurtNotInjuryCoroutine = null;

            FullControlVelocitying = false;
            if (HasGetEnablePos)
            {
                transform.position = OnEnablePos;
            }

            HurtBackFactor = 1;//受伤击退因数
            HurtStateFactor = 1;//受伤僵直时间因数

        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            AttackHittedEffectCorotine = null;
            HurtNotInjuryCoroutine = null;
            mRigidbody = null;
            effect = null;
            On玩家受伤Event.RemoveListenner(HurtHitBack);
        }

        private void FixedUpdate()
        {
            //重力计算
            if (FullControlVelocitying || Attacking) return;
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
            OnEnablePos = transform.position;
        }

        /// <summary>
        /// 用于顿帧使用，该函数将使玩家速度按ratio比例降低ControlTime时间后恢复原速
        /// 同时调用顺序执行
        /// </summary>
        public void FullControlVelocity(float ratio, float FreezeTime, float SecondFreezeTime)
        {
            //mRigidbody.velocity *= ratio;
            AttackHittedEffectList.Add(FreezeTime);
            if (AttackHittedEffectCorotine == null)
            {
                AttackHittedEffectCorotine = StartCoroutine(ExecuteAttackHittedEvent(ratio, SecondFreezeTime));
            }
        }

        /// <summary>
        /// 给一个推力
        /// </summary>
        public void GetAPush(Vector2 force)
        {
            if (FullControlVelocitying)
                mRigidbody.velocity = force;
            //StartCoroutine(GetAPushCoroutine(force));
        }

        /// <summary>
        /// 给一个横向击退力
        /// </summary>
        public void GetHittedBackForce(float force)
        {
            if (CanotHitBack) return;
            // if (FullControlVelocitying)
            //     mRigidbody.velocity = force;
            //StartCoroutine(GetAPushCoroutine(force));
            if (FullControlVelocitying && currentHittedBackForce < force)
            {
                currentHittedBackForce = force;
                SetVelocity(SetCoord.X, -transform.localScale.x * currentHittedBackForce);
            }
        }

        public void StopFullControlVelocity()
        {
            if (AttackHittedEffectCorotine != null)
            {
                StopCoroutine(AttackHittedEffectCorotine);
            }
            //StopAllCoroutines();
            AttackHittedEffectCorotine = null;
            AttackHittedEffectList.Clear();
            FullControlVelocitying = false;
            speedRatio = 1;
        }

        //受伤扣血前，获取攻击对象
        public void HurtHitBack(GameObject attacker)
        {
            Attacker = attacker;
        }

        public void StartHurt()
        {
            SetVelocity(SetCoord.Y, HurtJumpForce * HurtBackFactor);
            var direction = base.transform.position.x - Attacker.transform.position.x;
            if (direction == 0)
                direction = -1;
            SetVelocity(SetCoord.X, Mathf.Sign(direction) * HurtHoriontalForce * HurtBackFactor);
            //mRigidbody.velocity += Vector2.right * Math.Sign(transform.position.x - Attacker.transform.position.x) * HurtHoriontalForce;
        }

        public void EndHurt()
        {

        }

        public void Move(float AxesX)
        {
            Move(AxesX, MoveSpeed * property.BL移速增加倍率);
        }

        public void Attack(float AxesX)
        {
            Attacking = true;

            if (AxesX == 0)
                SetVelocity(SetCoord.X, SetScale(AxesX) * attackMoveSpeed);
            //SetVelocity(SetCoord.X, SetScale(AxesX) * attackMoveSpeed * property.BL移速增加倍率);
            else
            {
                SetVelocity(SetCoord.X, SetScale(AxesX) * (attackMoveSpeed + attackExtraMoveSpeed));
                //SetVelocity(SetCoord.X, SetScale(AxesX) * (attackMoveSpeed * property.BL移速增加倍率 + attackExtraMoveSpeed));
            }

            //if (mRigidbody.velocity.y < -0.1f)
            SetVelocity(SetCoord.Y, 0f);
        }

        /// <summary>
        /// 在两个地方调用，
        /// </summary>
        public void AttackEnd()
        {
            currentHittedBackForce = 0;
            Attacking = false;
        }

        public void MoveWhenAttack(float AxesX)
        {
            //var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, 0, AttackDeceleration * property.BL移速增加倍率 * speedRatio * Time.fixedDeltaTime);
            var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, 0, AttackDeceleration * speedRatio * Time.fixedDeltaTime);
            SetVelocity(SetCoord.X, VelocityX);
        }

        public void UpAttackStart()
        {
            //Attacking = true;
            upAttackStartPosY = transform.position.y;
            SetVelocity(SetCoord.Y, upAttackSpeed);
        }

        public void UpAttackEnd()
        {
            SetVelocity(SetCoord.Y, 0);
            //Attacking = false;
        }

        public void DownAttackStart()
        {
            //Attacking = true;
            SetVelocity(SetCoord.Y, -downAttackSpeed);
        }

        public void DownAttackEnd()
        {
            //Attacking = false;
        }

        public void RollStart(float AxesX)
        {
            StopFullControlVelocity();
            SetVelocity(SetCoord.X, SetScale(AxesX) * RollStartSpeed * property.BL移速增加倍率);
            NotInjury(true);
        }

        public void RollEnd()
        {
            NotInjury(false);
        }

        public void Rolling(float AxesX)
        {
            float acceleration = 0;
            if (transform.localScale.x * AxesX > 0)
                acceleration = RollDeceleration - RollExtraAcceleration;
            else if (transform.localScale.x * AxesX < 0)
                acceleration = RollDeceleration + RollExtraDeceleration;
            else
                acceleration = RollDeceleration;
            var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, 0, acceleration * property.BL移速增加倍率 * Time.fixedDeltaTime);
            SetVelocity(SetCoord.X, VelocityX);
        }

        public void RollHold()
        {
            SetVelocity(SetCoord.X, transform.localScale.x * RollHoldSpeed * property.BL移速增加倍率);
        }

        public void Jump()
        {
            StopFullControlVelocity();
            SetVelocity(SetCoord.Y, JumpForce);
        }

        public void WallClimb()
        {
            mRigidbody.velocity = Vector2.zero;
            NotInjury(true);
            if (transform.localScale.x > 0)//向右
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
            NotInjury(true);

            WallClimbPos = new Vector2(transform.position.x,
                                        Mathf.Floor(WallSlideCheck_Font.Pos.y - 0.05f) + WallClimbYOffset1);
            EndWallClimbPos = new Vector2(transform.position.x + transform.localScale.x * WallClimbXOffset2,
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

        public void EndWallClimb()
        {
            IsFixToWallClimbPos = false;
            transform.position = EndWallClimbPos;
        }

        public void FixPosToWallClimbPos()
        {
            if (IsFixToWallClimbPos)
                transform.position = WallClimbPos;
        }

        public void HasWallClimbedEnd()
        {
            NotInjury(false);
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
            transform.position -= Vector3.up * DownFallOffsetY;
            SetVelocity(SetCoord.Y, -6f);
        }

        public void DecelerationWhenChangeableJump()
        {
            //Debug.Log($"{mRigidbody.velocity.y}");
            var speed = mRigidbody.velocity.y * ScaleChangeableJump;
            SetVelocity(SetCoord.Y, speed);
        }

        public void WallJump()
        {
            mRigidbody.velocity = Vector2.zero;
            var wallJumpSpeed = WallJumpSpeed;
            wallJumpSpeed.x *= transform.localScale.x * property.BL移速增加倍率;
            mRigidbody.velocity += wallJumpSpeed;
        }

        public void WallLeave()
        {
            var wallLeaveSpeed = WallLeaveSpeed;
            wallLeaveSpeed.x *= transform.localScale.x * property.BL移速增加倍率;
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
            SetScale(-(int)transform.localScale.x);
        }

        #endregion

        private void Move(float AxesX, float Speed)
        {
            float direction = 0;
            if (AxesX != 0)//加速
            {
                direction = Mathf.Sign(AxesX);

                SetScale((int)direction);

                var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, direction * Speed, MoveAcceleration * property.BL移速增加倍率 * Time.fixedDeltaTime);
                SetVelocity(SetCoord.X, VelocityX);
            }
            else
            {
                var VelocityX = Mathf.MoveTowards(mRigidbody.velocity.x, 0, Movedeceleration * property.BL移速增加倍率 * Time.fixedDeltaTime);
                SetVelocity(SetCoord.X, VelocityX);
            }
        }



        /// <summary>
        /// 设置单个坐标方向的速度
        /// </summary>
        public void SetVelocity(SetCoord coord, float value)
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
            if (direction == 0) return transform.localScale.x;
            if (transform.localScale.x * direction < 0)
            {
                var Scale = transform.localScale;
                Scale.x *= -1;
                transform.localScale = Scale;
            }
            return transform.localScale.x;
        }

        /// <summary>
        /// 使用这个函数进行玩家无敌操作
        /// </summary>
        /// <param name="Enable"></param>
        public void NotInjury(bool Enable)
        {
            if (Enable)
                StopHurtNorInjury();
            mColl.enabled = !Enable;
        }

        public void StartHurtNotInjury()
        {
            if (HurtNotInjuryCoroutine != null)
            {
                //StopCoroutine(HurtNotInjuryCoroutine);
                return;
            }
            HurtNotInjuryCoroutine = StartCoroutine(nameof(HurtNotInjury));
        }

        /// <summary>
        /// 当其他情况使角色无敌时，先关闭可能存在的受伤无敌协程
        /// </summary>
        private void StopHurtNorInjury()
        {
            if (HurtNotInjuryCoroutine != null)
                StopCoroutine(HurtNotInjuryCoroutine);
        }

        /// <summary>
        /// 顺序执行减速效果协程
        /// </summary>
        IEnumerator ExecuteAttackHittedEvent(float ratio, float SecondFreezeTime)
        {
            bool firstFreeze = true;

            FullControlVelocitying = true;
            while (AttackHittedEffectList.Count > 0)
            {
                float freezeTime;
                if (firstFreeze)
                {
                    firstFreeze = false;
                    freezeTime = AttackHittedEffectList[0];
                }
                else
                    freezeTime = SecondFreezeTime;
                AttackHittedEffectList.RemoveAt(0);
                yield return StartCoroutine(AttackHittedEvent(ratio, freezeTime, SecondFreezeTime));
            }

            FullControlVelocitying = false;
            AttackHittedEffectCorotine = null;
        }
        /// <summary>
        /// 减速效果协程
        /// </summary>
        IEnumerator AttackHittedEvent(float ratio, float FreezeTime, float SecondFreezeTime)
        {
            // speedRatio = ratio;
            // var velocity = mRigidbody.velocity;
            // mRigidbody.velocity *= ratio;
            if (FreezeTime == SecondFreezeTime)
                yield return waitForSecondFreezeTime;
            else
                yield return waitForAttackHittedFreezeTime;
            // if (playerFSM.animManager.IsAttacking)
            //     mRigidbody.velocity = velocity / 2;
            //speedRatio = 1;
            yield return waitForIntervalHittedEffect;
        }

        IEnumerator GetAPushCoroutine(Vector2 force)
        {
            while (FullControlVelocitying)
            {
                yield return null;
            }
            mRigidbody.velocity += force;
        }

        IEnumerator HurtNotInjury()
        {
            mColl.enabled = false;
            yield return waitForHurtNotInjuryTime;
            mColl.enabled = true;

            HurtNotInjuryCoroutine = null;
        }
    }
}