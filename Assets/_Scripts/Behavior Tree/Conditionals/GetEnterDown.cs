using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.InputSystem;

namespace Adv
{
    [TaskCategory("Custom/Input")]
    [TaskDescription("检测回车键的输入")]
    public class GetEnterDown : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            return Keyboard.current.enterKey.isPressed ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
