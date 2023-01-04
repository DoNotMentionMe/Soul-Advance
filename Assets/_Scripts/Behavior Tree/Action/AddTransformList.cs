using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    public class AddTransformList : Action
    {
        [SerializeField] SharedTransform TargetTransform;
        [SerializeField] SharedTransformList StoreTransformList;

        public override TaskStatus OnUpdate()
        {
            StoreTransformList.Value.Add(TargetTransform.Value);
            return TaskStatus.Success;
        }
    }
}
