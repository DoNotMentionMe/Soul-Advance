using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    public class DelayCall_SetBool : Action
    {
        [SerializeField] SharedFloat DelayTime;
        [SerializeField] SharedBool Storesult;
        [SerializeField] SharedBool SetValue;

        public override TaskStatus OnUpdate()
        {
            DG.Tweening.DOVirtual.DelayedCall(DelayTime.Value, () => { Storesult.Value = SetValue.Value; });
            return TaskStatus.Success;
        }
    }
}
