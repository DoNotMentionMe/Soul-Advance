using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    public class TestPoolManager : MonoBehaviour
    {
        [SerializeField] EnemyGeneratedData enemy;
        [SerializeField] Vector3 releasePos;

        [Button]
        public void PoolReleaseNoPos()
        {
            PoolManager.Instance.ReleaseEnemy(enemy);
        }

        [Button]
        public void PoolReleaseHasPos()
        {
            PoolManager.Instance.ReleaseEnemy(enemy, releasePos);
        }
    }
}
