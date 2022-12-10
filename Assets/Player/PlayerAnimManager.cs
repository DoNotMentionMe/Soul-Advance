using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;

namespace Adv
{
    public class PlayerAnimManager : MonoBehaviour
    {
        public string CurrentAnimName => currentAnim.Name;
        public bool IsAttacking { get; set; }

        [SerializeField] apPortrait mApPortrait;
        [SerializeField] SpriteRenderer WeaponSpriteRenderer;
        [SerializeField] TrailRenderer 刀光;
        [SerializeField] Transform playerTransform;
        [SerializeField] Trigger2D 攻击碰撞体;

        private List<apAnimPlayData> animList = new List<apAnimPlayData>();
        private apAnimPlayData lastAnim;
        private apAnimPlayData currentAnim;

        private void Start()
        {
            animList = mApPortrait.AnimationPlayDataList;
            mApPortrait.Initialize();
        }

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

        public void CurrentAnimSpeedSlowDown(float speed) => currentAnim?.SetSpeed(speed);
        public bool IsAnimEnded(AnimName animName) => animList[((int)animName)].PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended;
        public bool IsAnimNone(AnimName animName) => animList[((int)animName)].PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.None;

        private void GetLastAnim()
        {
            CurrentAnimSpeedSlowDown(1);
            WeaponSpriteRenderer.sortingOrder = 15;
            lastAnim = currentAnim;
            IsAttacking = false;
        }

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
            攻击碰撞体.SetCollEnable(true);
            WeaponSpriteRenderer.sortingOrder = 15;
            IsAttacking = true;
            刀光.emitting = true;
        }

        private void Attack1End()
        {
            攻击碰撞体.SetCollEnable(false);
            WeaponSpriteRenderer.sortingOrder = 0;
            刀光.emitting = false;
        }

        private void Attack2Start()
        {
            攻击碰撞体.SetCollEnable(true);
            WeaponSpriteRenderer.sortingOrder = 0;
            IsAttacking = true;
            刀光.emitting = true;
        }

        private void Attack2End()
        {
            攻击碰撞体.SetCollEnable(false);
            WeaponSpriteRenderer.sortingOrder = 15;
            刀光.emitting = false;
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
