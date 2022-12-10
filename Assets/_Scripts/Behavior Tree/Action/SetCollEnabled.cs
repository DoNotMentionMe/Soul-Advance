using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom/Action")]
    public class SetCollEnabled : Action
    {
        [SerializeField] SharedTrigger2D mColl2D;
        [SerializeField] bool Enable;

        public override TaskStatus OnUpdate()
        {
            mColl2D.Value.SetCollEnable(Enable);
            return TaskStatus.Success;
        }
    }
}
