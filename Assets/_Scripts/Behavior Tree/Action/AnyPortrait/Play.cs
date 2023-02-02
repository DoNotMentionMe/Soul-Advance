using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Adv
{
    [TaskCategory("Custom/ApPortrait")]
    [TaskDescription("骨骼动画Play播放动画函数")]
    public class Play : Action
    {
        [SerializeField] SharedApPortrait anim;
        [SerializeField] string animName;

        public override void OnStart()
        {
            anim.Value.Play(animName);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
