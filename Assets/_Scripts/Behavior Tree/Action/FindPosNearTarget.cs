using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("(未施工)找到目标附近(一定距离)的点，同时尽量接近自己")]
    public class FindPosNearTarget : Action
    {
        [SerializeField] SharedVector3 TargetPos;
        [SerializeField] SharedFloat DistanceWithTarget;

        public override TaskStatus OnUpdate()
        {


            return base.OnUpdate();
        }
    }
}
