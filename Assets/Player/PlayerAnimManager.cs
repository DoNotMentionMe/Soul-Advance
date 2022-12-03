using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;

namespace Adv
{
    public class PlayerAnimManager : MonoBehaviour
    {
        [SerializeField] apPortrait mApPortrait;

        private List<apAnimPlayData> animList = new List<apAnimPlayData>();

        private void Start()
        {
            animList = mApPortrait.AnimationPlayDataList;
        }

        public void Play(AnimName animName)
        {
            mApPortrait.Play(animList[((int)animName)]);
        }

        public void CrossFade(AnimName animName, float fadeTime = 0.1f)
        {
            mApPortrait.CrossFade(animList[((int)animName)], fadeTime);
        }

        public void CrossFadeNextFrame(AnimName animName, float fadeTime = 0.1f)
        {
            mApPortrait.CrossFade(animList[((int)animName)], fadeTime);
        }

        public void CrossFadeQueued(AnimName animName, float fadeTime = 0.1f)
        {
            mApPortrait.CrossFadeQueued(animList[((int)animName)], fadeTime);
        }

        public bool IsAnimEnded(AnimName animName) => animList[((int)animName)].PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended;
        public bool IsAnimNone(AnimName animName) => animList[((int)animName)].PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.None;

    }

    public enum AnimName
    {
        Walk,
        Idle,
        JumpUp,
        JumpDown,
        Roll,
        Roll2,
        WallSlide,
        WallSlide2,
        WallJump,
        WallClimb,
        HasWallClimbed,
        Attack,
    }
}
