using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using System.Collections.Generic;

namespace Adv
{
    public class RemoveTransformList : Action
    {
        [SerializeField] SharedTransformList TargetTransfromList;
        [SerializeField] SharedTransform RemoveTransform;

        private List<Transform> targetTransformList;
        private Transform removeTransform;

        public override void OnStart()
        {
            if (targetTransformList == null)
            {
                targetTransformList = TargetTransfromList.Value;
            }
            removeTransform = RemoveTransform.Value;
        }

        public override TaskStatus OnUpdate()
        {
            if (targetTransformList.Contains(removeTransform))
            {
                targetTransformList.Remove(removeTransform);
                return TaskStatus.Success;
            }
            else
            {
                Debug.LogError("List不包含移除目标");
                return TaskStatus.Failure;
            }
        }
    }
}
