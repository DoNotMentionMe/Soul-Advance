using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    public class ClearTransformList : Action
    {
        [SerializeField] SharedTransformList TargetTransformList;

        public override TaskStatus OnUpdate()
        {
            TargetTransformList.Value.Clear();
            return TaskStatus.Success;
        }
    }
}
