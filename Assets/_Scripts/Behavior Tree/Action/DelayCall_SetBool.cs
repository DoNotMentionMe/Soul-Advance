using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using DG.Tweening;

namespace Adv
{
    public class DelayCall_SetBool : Action
    {
        [Tooltip("勾选该项需要使用StoreTween，并且当Tween不为空时，会先停止已存在的Tween再开始新的Tween")]
        public bool IsStoreTween = false;
        public SharedFloat DelayTime;
        public SharedBool Storesult;
        public SharedBool SetValue;
        public SharedTween StoreTween;

        public override TaskStatus OnUpdate()
        {
            if (IsStoreTween)
            {
                if (StoreTween.Value != null)
                    StoreTween.Value.Kill();
                StoreTween.Value = DOVirtual.DelayedCall(DelayTime.Value, () => { Storesult.Value = SetValue.Value; });
            }
            else
            {
                DOVirtual.DelayedCall(DelayTime.Value, () => { Storesult.Value = SetValue.Value; });
            }
            return TaskStatus.Success;
        }
    }
}
