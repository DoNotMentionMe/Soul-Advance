
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom/Conditional")]
    public class Trigger2DCheck : Conditional
    {
        [SerializeField] SharedTrigger2D trigger2D;
        [SerializeField] SharedBool BoolComparison;
        [Space]
        [Header("自定义bool变量判定")]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("是否检测trigger2D和自定义bool类型变量")]
        [SerializeField] bool CheckCustomBool = false;
        [SerializeField] SharedBool CustomBool;
        [SerializeField] SharedBool CustomBoolComparison;
        public override TaskStatus OnUpdate()
        {
            return (trigger2D.Value.IsTriggered == BoolComparison.Value
                    || (CheckCustomBool && CustomBool.Value == CustomBoolComparison.Value))
            ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
