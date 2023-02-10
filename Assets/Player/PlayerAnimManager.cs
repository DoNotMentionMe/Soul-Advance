using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;
using DG.Tweening;

namespace Adv
{
    public class PlayerAnimManager : MonoBehaviour
    {
        public string CurrentAnimName => currentAnim.Name;
        public bool IsAttacking { get; set; }

        public Animator effectAnim;
        [SerializeField] apPortrait mApPortrait;
        [SerializeField] SpriteRenderer WeaponSpriteRenderer;
        [SerializeField] Trigger2D 攻击碰撞体;
        [SerializeField] PlayerFSM playerFSM;
        [SerializeField] PlayerProperty property;
        [SerializeField] FloatEventChannel On攻击动画速度变更Event;
        [SerializeField] FloatEventChannel On移动动画速度变更Event;

        private PlayerController playerController;
        private List<apAnimPlayData> animList = new List<apAnimPlayData>();
        private List<apAnimPlayData> AttackAnimList = new List<apAnimPlayData>();
        private List<apAnimPlayData> MoveAnimList = new List<apAnimPlayData>();
        private List<float> AttackHittedEffectList = new List<float>();//用来记录执行间隔和执行特效次数
        private Coroutine AttackHittedEffectCorotine;//攻击命中顺序执行协程
        private WaitForSecondsRealtime waitForIntervalHittedEffect;
        private WaitForSecondsRealtime waitForAttackHittedFreezeTime;
        private WaitForSecondsRealtime waitForSecondFreezeTime;
        private apAnimPlayData lastAnim;
        private apAnimPlayData currentAnim;
        private bool ControlAnimSpeeding;

        private void Awake()
        {
            mApPortrait.Initialize();
            InitialAllAnim();
            On攻击动画速度变更Event.AddListener(On攻击动画速度变更);
            On移动动画速度变更Event.AddListener(On移动动画速度变更);
            playerController = playerFSM.ctler;
        }

        private void Start()
        {
            waitForIntervalHittedEffect = new WaitForSecondsRealtime(playerFSM.effect.IntervalOfHittedEffect);
            waitForAttackHittedFreezeTime = new WaitForSecondsRealtime(playerFSM.effect.AttackHittedFreezeTime);
            waitForSecondFreezeTime = new WaitForSecondsRealtime(playerFSM.effect.SecondFreezeTime);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            AttackHittedEffectCorotine = null;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            AttackHittedEffectCorotine = null;
            On攻击动画速度变更Event.RemoveListenner(On攻击动画速度变更);
            On移动动画速度变更Event.RemoveListenner(On移动动画速度变更);
            playerController = null;
        }

        private void On攻击动画速度变更(float speed)
        {
            SetAttackAnimSpeed(property.BL攻速倍率);
        }

        private void On移动动画速度变更(float speed)
        {
            SetMoveAnimSpeed(property.BL移速增加倍率);
        }

        private void InitialAllAnim()
        {
            animList = mApPortrait.AnimationPlayDataList;
            AttackAnimList.Add(GetApAnimPlayData(AnimName.GAttack1));
            AttackAnimList.Add(GetApAnimPlayData(AnimName.GAttack2));
            AttackAnimList.Add(GetApAnimPlayData(AnimName.GAttack3));
            AttackAnimList.Add(GetApAnimPlayData(AnimName.GAttack4));
            MoveAnimList.Add(GetApAnimPlayData(AnimName.Walk));
            MoveAnimList.Add(GetApAnimPlayData(AnimName.Roll));
            MoveAnimList.Add(GetApAnimPlayData(AnimName.JumpDown));
            MoveAnimList.Add(GetApAnimPlayData(AnimName.JumpUp));
            MoveAnimList.Add(GetApAnimPlayData(AnimName.WallJump));
        }

        #region 控制函数

        /// <summary>
        /// 控制玩家动画，用于命中顿帧，同时调用顺序执行
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="FreezeTime"></param>
        public void CurrentAnimSpeedSlowDownForAWhile(float speed, float FreezeTime, float SecondFreezeTime)
        {
            AttackHittedEffectList.Add(FreezeTime);
            if (AttackHittedEffectCorotine == null)
            {
                //Debug.Log($"进行一次命中协程");
                AttackHittedEffectCorotine = StartCoroutine(ExecuteAttackHittedEvent(speed, SecondFreezeTime));
            }
        }

        public void SetControlParamInt(string ParamName, int ParamInt) => mApPortrait.SetControlParamInt(ParamName, ParamInt);
        public void CurrentAnimSpeedSlowDown(float speed)
        {
            mApPortrait.SetAnimationSpeed(speed * property.BL攻速倍率);
            effectAnim.speed = speed * property.BL攻速倍率;
        }
        public void SetAttackAnimSpeed(float speed)
        {
            foreach (var anim in AttackAnimList)
            {
                anim.SetSpeed(speed);
            }
            effectAnim.speed = speed;
        }
        public void SetMoveAnimSpeed(float speed)
        {
            foreach (var anim in MoveAnimList)
            {
                anim.SetSpeed(speed);
            }
        }
        public bool IsAnimEnded(AnimName animName) => animList[((int)animName)].PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended;
        public bool IsAnimNone(AnimName animName) => animList[((int)animName)].PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.None;
        public apAnimPlayData GetApAnimPlayData(AnimName animName) => animList[((int)animName)];

        #endregion


        #region 播放函数
        public void Play(AnimName animName)
        {
            GetLastAnim();
            currentAnim = mApPortrait.Play(animList[((int)animName)]);
        }

        public void CrossFade(AnimName animName, float fadeTime = 0.1f)
        {
            GetLastAnim();
            currentAnim = mApPortrait.CrossFade(animList[((int)animName)], fadeTime);
        }

        public void CrossFadeNextFrame(AnimName animName, float fadeTime = 0.1f)
        {
            GetLastAnim();
            currentAnim = mApPortrait.CrossFade(animList[((int)animName)], fadeTime);
        }

        public void CrossFadeQueued(AnimName animName, float fadeTime = 0.1f)
        {
            GetLastAnim();
            currentAnim = mApPortrait.CrossFadeQueued(animList[((int)animName)], fadeTime);
        }

        private void GetLastAnim()
        {
            //StopAllCoroutines();
            //CurrentAnimSpeedSlowDown(1);
            SetAttackAnimSpeed(property.BL攻速倍率);
            SetMoveAnimSpeed(property.BL移速增加倍率);
            //ControlAnimSpeeding = false;
            effectAnim.Play("Idle");
            攻击碰撞体.SetCollEnable(false);
            WeaponSpriteRenderer.sortingOrder = 15;
            lastAnim = currentAnim;
            IsAttacking = false;
        }

        #endregion

        #region 动画事件
        private void SlideToSlide2()
        {
            if (CurrentAnimName == AnimName.WallSlide.ToString())
            {
                lastAnim = currentAnim;
                currentAnim = mApPortrait.Play(animList[((int)AnimName.WallSlide2)]);
            }
        }

        private void SlideStart()
        {
            //修改武器图片图层排序--0
            WeaponSpriteRenderer.sortingOrder = 0;
        }

        private void GAttack2Start()
        {
            //攻击碰撞体.SetCollEnable(true);
            WeaponSpriteRenderer.sortingOrder = 0;
            IsAttacking = true;
        }
        private void GAttack()
        {
            //攻击碰撞体.SetCollEnable(true);
            WeaponSpriteRenderer.sortingOrder = 15;
            IsAttacking = true;
        }
        private void GAttackEnd()
        {
            playerController.Attacking = false;
        }
        #endregion

        /// <summary>
        /// 顺序执行特效协程
        /// </summary>
        IEnumerator ExecuteAttackHittedEvent(float speed, float SecondFreezeTime)
        {
            bool firstFreeze = true;
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
                yield return StartCoroutine(AttackHittedEvent(speed, freezeTime, SecondFreezeTime));
            }
            AttackHittedEffectCorotine = null;
        }
        /// <summary>
        /// 特效协程
        /// </summary>
        IEnumerator AttackHittedEvent(float speed, float FreezeTime, float SecondFreezeTime)
        {
            ControlAnimSpeeding = true;
            //CurrentAnimSpeedSlowDown(speed);
            SetAttackAnimSpeed(speed);
            // DOVirtual.DelayedCall(FreezeTime, () =>
            // {
            // });
            if (FreezeTime == SecondFreezeTime)
            {
                //Debug.Log($"secondsAnimStop");
                yield return waitForSecondFreezeTime;
            }
            else
            {
                //Debug.Log($"FirstAnimStop,{Time.time}");
                yield return waitForAttackHittedFreezeTime;
                //Debug.Log($"FirstAnimStopEnd,{Time.time}");
            }
            //CurrentAnimSpeedSlowDown(1);
            SetAttackAnimSpeed(property.BL攻速倍率);
            ControlAnimSpeeding = false;
            yield return waitForIntervalHittedEffect;
        }
    }

    public enum AnimName
    {
        Idle,
        Walk,
        JumpUp,
        JumpDown,
        Roll,
        WallSlide,
        WallSlide2,
        WallJump,
        WallClimb,
        HasWallClimbed,
        Attack1,
        Attack2,
        GAttack1,
        GAttack2,
        GAttack3,
        GAttack4,
        UpAttack,
        DownAttack,
        DashAttack,
    }
}
