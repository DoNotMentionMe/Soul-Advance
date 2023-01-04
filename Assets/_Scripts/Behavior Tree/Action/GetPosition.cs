using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("通过TargetTransform获取Position")]
    public class GetPosition : Action
    {
        [SerializeField] SharedTransform TargetTransform;
        [SerializeField] SharedVector3 StoreVector3;

        public override TaskStatus OnUpdate()
        {
            StoreVector3.Value = TargetTransform.Value.position;
            return TaskStatus.Success;
        }
    }
}
