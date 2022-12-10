using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("将resultStore设置为localSacle的X值")]
    public class SetIntEqualScaleX : Action
    {
        [SerializeField] SharedTransform mTransform;
        [SerializeField] SharedInt resultStore;

        public override void OnStart()
        {
            resultStore.Value = (int)mTransform.Value.localScale.x;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
