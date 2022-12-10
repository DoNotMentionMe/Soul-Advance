using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom/Collider2D")]
    public class SetEnabled : Action
    {
        [SerializeField] SharedCollider2D coll2D;
        [SerializeField] SharedBool enabled;

        public override TaskStatus OnUpdate()
        {

            coll2D.Value.enabled = enabled.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            coll2D.Value = null;
        }
    }
}
