using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using AnyPortrait;

namespace Adv
{
    [TaskCategory("Custom/ApPortrait")]
    [TaskDescription("骨骼动画Play播放动画函数")]
    public class WaitAnimEnd : Action
    {
        [SerializeField] SharedApPortrait anim;
        [SerializeField] string animName;
        private apAnimPlayData animPlayData;
        private bool GetPlayData = false;

        public override void OnStart()
        {
            if (!GetPlayData)
            {
                animPlayData = anim.Value.GetAnimationPlayData(animName);
                GetPlayData = true;
            }
        }

        public override TaskStatus OnUpdate()
        {
            return animPlayData.PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnReset()
        {
            animPlayData = null;
            GetPlayData = false;
        }
    }
}
