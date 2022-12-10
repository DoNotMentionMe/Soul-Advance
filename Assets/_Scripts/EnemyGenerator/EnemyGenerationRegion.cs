using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class EnemyGenerationRegion : MonoBehaviour
    {
        public float 可生成总数 => m可生成总数;
        public Vector2 Left => left.position;
        public Vector2 Right => right.position;
        [SerializeField] Transform left;
        [SerializeField] Transform right;

        /// <summary>
        /// 当前可生成数是否满足敌人占用，满足则扣除占用生成敌人并返回真
        /// </summary>
        /// <param name="occupation"></param>
        /// <returns></returns>
        public bool CanGenerateOnThisRegion(EnemyGeneratedData EnemyPrefab)
        {
            if (当前可生成数 >= EnemyPrefab.Occupation)
            {
                当前可生成数 -= EnemyPrefab.Occupation;
                GenerateEnemy(EnemyPrefab);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 在left和right之间随机位置生成敌人
        /// </summary>
        /// <param name="EnemyPrefab"></param>
        public void GenerateEnemy(EnemyGeneratedData EnemyPrefab)
        {
            var x = Random.Range(left.position.x, right.position.x);
            var y = mTransform.position.y + EnemyPrefab.GeneratedOffsetY;
            var generatePos = Vector2.right * x + Vector2.up * y;
            PoolManager.Instance.Release(EnemyPrefab.gameObject, generatePos).GetComponent<EnemyGeneratedData>().GetRegion(this);
        }

        /// <summary>
        /// 敌人死亡时调用
        /// </summary>
        /// <param name="occupation"></param>
        public void ReleaseOccupation(float occupation)
        {
            当前可生成数 += occupation;
        }

        private float m可生成总数;
        private float 当前可生成数;
        private Transform mTransform;

        private void Awake()
        {
            m可生成总数 = right.position.x - left.position.x;
            mTransform = transform;
            当前可生成数 = m可生成总数;
        }

        private void Start()
        {
            Resources.Load<EnemyGenerationRegionEventChannel>("Event Channels/EnemyGenerationRegionEventChannel_SendEnemyGenerationRegion").Broadcast(this);
        }

        private void OnDestroy()
        {
            mTransform = null;
        }


    }
}
