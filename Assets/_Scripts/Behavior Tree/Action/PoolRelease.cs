using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace Adv
{
    public class PoolRelease : Action
    {
        [SerializeField] SharedGameObject ReleaseObj;
        [SerializeField] bool UsePos;
        [SerializeField] bool UseLocalScale;
        [SerializeField] SharedVector3 Pos;
        [SerializeField] SharedVector3 LocalScale;

        private GameObject releaseObj;

        public override void OnStart()
        {
            if (releaseObj == null)
                releaseObj = ReleaseObj.Value;
        }

        public override TaskStatus OnUpdate()
        {
            if (!UsePos && !UseLocalScale)
                PoolManager.Instance.Release(releaseObj);
            else if (UsePos && !UseLocalScale)
                PoolManager.Instance.Release(releaseObj, Pos.Value);
            else if (!UsePos && UseLocalScale)
                PoolManager.Instance.Release(releaseObj, releaseObj.transform.position, Quaternion.identity, LocalScale.Value);
            else if (UsePos && UseLocalScale)
            {
                PoolManager.Instance.Release(releaseObj, Pos.Value, Quaternion.identity, LocalScale.Value);
            }
            return TaskStatus.Success;
        }
    }
}
