using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    public class Jump : Action
    {
        [SerializeField] SharedRigidbody2D mRigidbody;
        [SerializeField] SharedFloat JumpForce;

        public override TaskStatus OnUpdate()
        {
            var velocity = mRigidbody.Value.velocity;
            velocity.y = JumpForce.Value;
            mRigidbody.Value.velocity = velocity;
            return TaskStatus.Success;
        }
    }
}
