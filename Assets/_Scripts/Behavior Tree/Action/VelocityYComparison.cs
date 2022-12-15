using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custon")]
    [TaskDescription("一直等待y轴速度比较结果, 直到成立")]
    public class VelocityYComparison : Action
    {
        private enum Operation
        {
            LessThan,
            LessThanOrEqualTo,
            EqualTo,
            NotEqualTo,
            GreaterThanOrEqualTo,
            GreaterThan
        }
        [SerializeField] Operation operation;
        [SerializeField] SharedRigidbody2D mRigidbody;
        [SerializeField] SharedFloat ComparisonVale;

        public override TaskStatus OnUpdate()
        {
            switch (operation)
            {
                case Operation.LessThan:
                    return mRigidbody.Value.velocity.y < ComparisonVale.Value ? TaskStatus.Success : TaskStatus.Running;
                case Operation.LessThanOrEqualTo:
                    return mRigidbody.Value.velocity.y <= ComparisonVale.Value ? TaskStatus.Success : TaskStatus.Running;
                case Operation.EqualTo:
                    return Mathf.Approximately(mRigidbody.Value.velocity.y, ComparisonVale.Value) ? TaskStatus.Success : TaskStatus.Running;
                case Operation.NotEqualTo:
                    return !Mathf.Approximately(mRigidbody.Value.velocity.y, ComparisonVale.Value) ? TaskStatus.Success : TaskStatus.Running;
                case Operation.GreaterThanOrEqualTo:
                    return mRigidbody.Value.velocity.y >= ComparisonVale.Value ? TaskStatus.Success : TaskStatus.Running;
                case Operation.GreaterThan:
                    return mRigidbody.Value.velocity.y > ComparisonVale.Value ? TaskStatus.Success : TaskStatus.Running;
            }
            return TaskStatus.Failure;
        }
    }
}
