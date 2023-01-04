using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using NaughtyAttributes;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("旋转Z轴朝向TargetLookAt")]
    public class LookAt2D : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("如果勾选LookAtPlayerFSM，使用TargetLookAtPlayerFSM")]
        [SerializeField] bool LookAtPlayerFSM = false;
        [SerializeField] SharedTransform TargetTransform;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("如果勾选LookAtPlayerFSM，此项无效，使用TargetLookAtPlayerFSM")]
        [SerializeField] SharedTransform TargetLookAtTransform;
        [SerializeField] SharedPlayerFSM TargetLookAtPlayerFSM;
        private Transform targetTransform;

        public override void OnStart()
        {
            if (targetTransform == null)
                targetTransform = TargetTransform.Value;
        }

        public override TaskStatus OnUpdate()
        {
            Vector3 direction;
            if (LookAtPlayerFSM)
                direction = TargetLookAtPlayerFSM.Value.transform.position - targetTransform.position;
            else
                direction = TargetLookAtTransform.Value.position - targetTransform.position;
            targetTransform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);

            return TaskStatus.Success;
        }
    }
}
