using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("判断与TargetTransform之间的距离")]
    public class DistanceComparisonWithTransform : Conditional
    {
        public enum ComparisonModes
        {
            LessThan,
            LessThanOrEqualTo,
            EqualTo,
            NotEqualTo,
            GreaterThanOrEqualTo,
            GreaterThan
        }
        [SerializeField] ComparisonModes comparisonMode;
        [SerializeField] SharedTransform TargetTransform;
        [SerializeField] SharedFloat ComparisonDistance;

        public override TaskStatus OnUpdate()
        {
            var distance = Vector3.Distance(TargetTransform.Value.position, transform.position);

            switch (comparisonMode)
            {
                case ComparisonModes.LessThan:
                    return distance < ComparisonDistance.Value ? TaskStatus.Success : TaskStatus.Failure;
                case ComparisonModes.LessThanOrEqualTo:
                    return distance <= ComparisonDistance.Value ? TaskStatus.Success : TaskStatus.Failure;
                case ComparisonModes.EqualTo:
                    return distance == ComparisonDistance.Value ? TaskStatus.Success : TaskStatus.Failure;
                case ComparisonModes.NotEqualTo:
                    return distance != ComparisonDistance.Value ? TaskStatus.Success : TaskStatus.Failure;
                case ComparisonModes.GreaterThanOrEqualTo:
                    return distance >= ComparisonDistance.Value ? TaskStatus.Success : TaskStatus.Failure;
                case ComparisonModes.GreaterThan:
                    return distance > ComparisonDistance.Value ? TaskStatus.Success : TaskStatus.Failure;
            }
            return TaskStatus.Failure;
        }
    }
}
