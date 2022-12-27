using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Adv
{
    [TaskCategory("Custom/ApPortrait")]
    [TaskDescription("骨骼动画Play播放动画函数")]
    public class SetControlParamInt : Action
    {
        [SerializeField] SharedApPortrait anim;
        [SerializeField] string controlParamName;
        [SerializeField] int setValue;

        public override TaskStatus OnUpdate()
        {
            anim.Value.SetControlParamInt(controlParamName, setValue);
            return TaskStatus.Success;
        }
    }
}
