using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    public class ScaleXFlip : Action
    {
        [SerializeField] SharedTransform mTransform;

        public override TaskStatus OnUpdate()
        {
            var Scale = mTransform.Value.localScale;
            Scale.x *= -1;
            mTransform.Value.localScale = Scale;
            return TaskStatus.Success;
        }
    }
}
