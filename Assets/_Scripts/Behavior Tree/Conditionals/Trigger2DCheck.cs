
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    public class Trigger2DCheck : Conditional
    {
        [SerializeField] SharedTrigger2D trigger2D;
        [SerializeField] SharedBool BoolComparison;
        public override TaskStatus OnUpdate()
        {
            return trigger2D.Value.IsTriggered == BoolComparison.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
