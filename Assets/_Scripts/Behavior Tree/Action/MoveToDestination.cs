using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using MoreMountains.Feedbacks;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("通过Feel插件的Destination实现移动效果")]
    public class MoveToDestination : Action
    {
        [SerializeField] SharedTransform TargetTransform;
        [SerializeField] SharedTransform DestinationTransform;
        [SerializeField] float duration;
        [SerializeField] MMF_Player feedbacks;
        [SerializeField] string Label;

        private bool IsStartMove = false;

        public override void OnStart()
        {
            var destination = feedbacks.GetFeedbackOfType<MMF_DestinationTransform>(Label);
            destination.TargetTransform = TargetTransform.Value;
            destination.Destination = DestinationTransform.Value;
            destination.Duration = duration;
            //feedbacks.Initialization();
            feedbacks.PlayFeedbacks();
            IsStartMove = true;
        }

        public override TaskStatus OnUpdate()
        {
            if (!IsStartMove) return TaskStatus.Running;
            if (feedbacks.IsPlaying)
            {
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
        public override void OnEnd()
        {
            IsStartMove = false;
        }
    }
}
