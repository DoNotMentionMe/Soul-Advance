using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class UnityEventInvoke : Action
    {
        [SerializeField] UnityEvent OnExecute = new UnityEvent();

        public override TaskStatus OnUpdate()
        {
            OnExecute?.Invoke();
            return TaskStatus.Success;
        }
    }
}
