using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    public class SetVelocityToTransform : Action
    {
        [SerializeField] SharedTransform TargetTransform;
        [SerializeField] SharedRigidbody2D mRigidbody2D;
        [SerializeField] SharedFloat Velocity;

        public override TaskStatus OnUpdate()
        {
            var direction = (TargetTransform.Value.position - transform.position).normalized;
            mRigidbody2D.Value.velocity = direction * Velocity.Value;
            return TaskStatus.Success;
        }
    }
}
