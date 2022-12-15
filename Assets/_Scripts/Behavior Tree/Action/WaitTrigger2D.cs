using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    public class WaitTrigger2D : Action
    {
        [SerializeField] SharedTrigger2D trigger2D;
        [SerializeField] SharedBool BoolComparison;

        public override TaskStatus OnUpdate()
        {
            return (trigger2D.Value.IsTriggered == BoolComparison.Value) ?
                    TaskStatus.Success : TaskStatus.Running;
        }
    }
}
