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

        public override void OnStart()
        {
            if (animPlayData == null)
                animPlayData = anim.Value.GetAnimationPlayData(animName);
        }

        public override TaskStatus OnUpdate()
        {
            return animPlayData.PlaybackStatus == apAnimPlayData.AnimationPlaybackStatus.Ended ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnReset()
        {
            animPlayData = null;
        }
    }
}
