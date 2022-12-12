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
        [SerializeField] Transform playerTransform;
        [SerializeField] Trigger2D 攻击碰撞体;

        private List<apAnimPlayData> animList = new List<apAnimPlayData>();
        private apAnimPlayData lastAnim;
        private apAnimPlayData currentAnim;
        private bool ControlAnimSpeeding;

        private void Start()
        {
            animList = mApPortrait.AnimationPlayDataList;
            mApPortrait.Initialize();
        }

        #region 控制函数

        public void CurrentAnimSpeedSlowDownForAWhile(float speed, float controlTime)
        {
            if (ControlAnimSpeeding) return;
            ControlAnimSpeeding = true;
            CurrentAnimSpeedSlowDown(speed);
            effectAnim.speed = speed;
            DOVirtual.DelayedCall(controlTime, () =>
            {
                CurrentAnimSpeedSlowDown(1);
                effectAnim.speed = 1;
                ControlAnimSpeeding = false;
            });
        }

        public void CurrentAnimSpeedSlowDown(float speed) => mApPortrait.SetAnimationSpeed(speed);
        public bool IsAnimEnded(AnimName animName) => animList[((int)animName)].PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended;
        public bool IsAnimNone(AnimName animName) => animList[((int)animName)].PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.None;

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
            CurrentAnimSpeedSlowDown(1);
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

        private void Attack1Start()
        {
            //攻击碰撞体.SetCollEnable(true);
            WeaponSpriteRenderer.sortingOrder = 15;
            IsAttacking = true;
        }

        private void Attack1End()
        {
            //攻击碰撞体.SetCollEnable(false);
            WeaponSpriteRenderer.sortingOrder = 0;
        }

        private void Attack2Start()
        {
            //攻击碰撞体.SetCollEnable(true);
            WeaponSpriteRenderer.sortingOrder = 0;
            IsAttacking = true;
        }

        private void Attack2End()
        {
            //攻击碰撞体.SetCollEnable(false);
            WeaponSpriteRenderer.sortingOrder = 15;
        }
        #endregion
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
    }
}
