using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    public class PoolReleaseAction : MonoBehaviour
    {
        [SerializeField] GameObject ReleaseObj;
        [SerializeField] bool ReleaseViaTransform;
        [ShowIf("ReleaseViaTransform")][SerializeField] Transform ReleaseTransform;
        [HideIf("ReleaseViaTransform")][SerializeField] Vector3 ReleasePos;
        public void Release()
        {
            if (ReleaseViaTransform)
                PoolManager.Instance.Release(ReleaseObj, ReleaseTransform.position);
            else
                PoolManager.Instance.Release(ReleaseObj, ReleasePos);
        }
    }
}
