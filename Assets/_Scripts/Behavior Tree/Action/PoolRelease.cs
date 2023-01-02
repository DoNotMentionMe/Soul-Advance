using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace Adv
{
    public class PoolRelease : Action
    {
        [SerializeField] GameObject ReleaseObj;
        [SerializeField] bool UsePos;
        [SerializeField] bool UseLocalScale;
        [SerializeField] SharedVector3 Pos;
        [SerializeField] SharedVector3 LocalScale;

        public override TaskStatus OnUpdate()
        {
            if (!UsePos && !UseLocalScale)
                PoolManager.Instance.Release(ReleaseObj);
            else if (UsePos && !UseLocalScale)
                PoolManager.Instance.Release(ReleaseObj, Pos.Value);
            else if (!UsePos && UseLocalScale)
                PoolManager.Instance.Release(ReleaseObj, ReleaseObj.transform.position, Quaternion.identity, LocalScale.Value);
            else if (UsePos && UseLocalScale)
            {
                PoolManager.Instance.Release(ReleaseObj, Pos.Value, Quaternion.identity, LocalScale.Value);
            }
            return TaskStatus.Success;
        }
    }
}
