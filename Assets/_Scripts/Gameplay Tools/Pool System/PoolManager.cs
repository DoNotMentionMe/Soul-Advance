using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    /// <summary>
    /// 缺点1，东西只会不断增多堆积在内存里，需要想办法减少多余对象
    /// 缺点2，如果不是特效类对象，像敌人这种可能存在很久的对象，会让对象池不断生成新对象而不会使用旧对象
    /// </summary>
    public class PoolManager : PersistentSingleton<PoolManager>
    {
        //每次增加新的对象池数组都需要在Awake()中初始化，在OnDestroy()中检查是否超出预定数量
        //1
        [SerializeField] EnemyPool[] Enemy;
        [SerializeField] Pool[] EnemyItem;
        [SerializeField] Pool[] Effect;
        [SerializeField] LDtkLevelPool[] LDtkLevel;

        static Dictionary<GameObject, Pool> dictionary;
        private Dictionary<string, EnemyPool> enemyPools;
        private Dictionary<LDtkLevel, LDtkLevelPool> lDtkLevelPools;

        private const string Clone = "(Clone)";
        private Coroutine OrderReleaseEnemyCoroutine;

        protected override void Awake()
        {
            base.Awake();
            LDtkLevel = new LDtkLevelPool[0];

            dictionary = new Dictionary<GameObject, Pool>();
            enemyPools = new Dictionary<string, EnemyPool>();
            lDtkLevelPools = new Dictionary<LDtkLevel, LDtkLevelPool>();

            //2
            InitializeEnemy(Enemy);
            Initialize(EnemyItem);
            Initialize(Effect);
        }
#if UNITY_EDITOR
        private void OnDestroy()
        {
            //3
            CheckPoolSize(Enemy);
            CheckPoolSize(EnemyItem);
            CheckPoolSize(Effect);
        }
#endif

        void CheckPoolSize(Pool[] pools)
        {
            foreach (var pool in pools)
            {
                if (pool.RuntimeSize > pool.Size)
                {
                    Debug.LogWarning(
                            string.Format("Pool: {0} has a runtime size {1} bigger than it initial size {2}",
                            pool.Prefab.name,
                            pool.RuntimeSize,
                            pool.Size)
                        );
                }
            }
        }
        void CheckPoolSize(EnemyPool[] pools)
        {
            foreach (var pool in pools)
            {
                if (pool.RuntimeSize > pool.Size)
                {
                    Debug.LogWarning(
                            string.Format("Pool: {0} has a runtime size {1} bigger than it initial size {2}",
                            pool.Prefab.name,
                            pool.RuntimeSize,
                            pool.Size)
                        );
                }
            }
        }


        void Initialize(Pool[] pools)
        {
            foreach (var pool in pools)
            {
#if UNITY_EDITOR
                if (dictionary.ContainsKey(pool.Prefab))
                {
                    Debug.LogError("Same prefab in multiple pools! Prefab: " + pool.Prefab.name);
                    continue;
                }
#endif

                dictionary.Add(pool.Prefab, pool);

                Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

                poolParent.parent = transform;
                pool.Initialize(poolParent);
            }
        }

        void InitializeEnemy(EnemyPool[] pools)
        {
            foreach (var pool in pools)
            {
#if UNITY_EDITOR
                if (enemyPools.ContainsKey(pool.Key))
                {
                    Debug.LogError("Same prefab in multiple pools! Prefab: " + pool.Prefab.name);
                    continue;
                }
#endif

                enemyPools.Add(pool.Key, pool);

                Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

                poolParent.parent = transform;
                pool.Initialize(poolParent);
            }
        }

        /// <summary>
        /// 根据输入预制体动态生成对象池
        /// </summary>
        public LDtkLevel ReleaseLDtkLevel(LDtkLevel prefab)
        {
            if (!lDtkLevelPools.ContainsKey(prefab))
            {
                Array.Resize(ref LDtkLevel, LDtkLevel.Length + 1);//扩容
                LDtkLevelPool pool = new LDtkLevelPool();
                pool.Prefab = prefab;//将预制体添加到对象池中
                pool.Size = 1;//TODO 建议设置为自定义初始数量
                LDtkLevel[LDtkLevel.Length - 1] = pool;
                lDtkLevelPools.Add(pool.Prefab, pool);

                Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

                poolParent.parent = transform;
                pool.Initialize(poolParent);
            }
            return lDtkLevelPools[prefab].PreparedLevel();
        }

        public LDtkLevel ReleaseLDtkLevel(LDtkLevel prefab, Vector3 position)
        {
            if (!lDtkLevelPools.ContainsKey(prefab))
            {
                Array.Resize(ref LDtkLevel, LDtkLevel.Length + 1);//扩容
                LDtkLevelPool pool = new LDtkLevelPool();
                pool.Prefab = prefab;//将预制体添加到对象池中
                pool.Size = 1;//TODO 建议设置为自定义初始数量
                LDtkLevel[LDtkLevel.Length - 1] = pool;
                lDtkLevelPools.Add(pool.Prefab, pool);

                Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

                poolParent.parent = transform;
                pool.Initialize(poolParent);
            }
            return lDtkLevelPools[prefab].PreparedLevel(position);
        }

        /// <summary>
        /// 敌人专属对象池：释放敌人
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public EnemyGeneratedData ReleaseEnemy(EnemyGeneratedData prefab, Vector3 position)
        {
            return enemyPools[string.Concat(prefab.name, Clone)].PreparedEnemy(position);
        }

        /// <summary>
        /// <para>函数描述：根据传入的<paramref name="prefab"></paramref>参数，返回对象池中预备好的游戏对象。</para>
        /// </summary>
        /// <param name="prefab">
        /// <para>输入参数说明：指定的游戏预制体</para>
        /// </param>
        /// <returns>
        /// <para>返回值说明：对象池中预备好的游戏对象</para>
        /// </returns>
        public GameObject Release(GameObject prefab)
        {
#if UNITY_EDITOR
            if (!dictionary.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

                return null;
            }
#endif

            return dictionary[prefab].PreparedObject();
        }
        /// <summary>
        /// <para>函数描述：根据传入的<paramref name="prefab"></paramref>参数，在<paramref name="position"></paramref>参数位置返回对象池中预备好的游戏对象。</para>
        /// </summary>
        /// <param name="prefab">
        /// <para>输入参数说明：指定的游戏预制体</para>
        /// </param>
        /// <param name="position">
        /// <para>输入参数说明：指定释放位置</para>
        /// </param>
        /// <returns></returns>
        public GameObject Release(GameObject prefab, Vector3 position)
        {
#if UNITY_EDITOR
            if (!dictionary.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

                return null;
            }
#endif

            return dictionary[prefab].PreparedObject(position);
        }

        /// <summary>
        /// <para>函数描述：根据传入的prefab参数，在postion参数位置返回对象池中预备好的旋转角度为rotation参数的游戏对象。</para>
        /// </summary>
        /// <param name="prefab">
        /// <para>输入参数说明：指定的游戏预制体</para>
        /// </param>
        /// <param name="position">
        /// <para>输入参数说明：指定释放位置</para>
        /// </param>
        /// <param name="rotation">
        /// <para>输入参数说明：指定释放对象的旋转值</para>
        /// </param>
        /// <returns></returns>
        public GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
        {
#if UNITY_EDITOR
            if (!dictionary.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

                return null;
            }
#endif

            return dictionary[prefab].PreparedObject(position, rotation);
        }
        /// <summary>
        /// <para>函数描述：根据传入的prefab参数，在postion参数位置返回对象池中预备好的旋转角度为rotation参数的游戏对象。</para>
        /// </summary>
        /// <param name="prefab">
        /// <para>输入参数说明：指定的游戏预制体</para>
        /// </param>
        /// <param name="position">
        /// <para>输入参数说明：指定释放位置</para>
        /// </param>
        /// <param name="rotation">
        /// <para>输入参数说明：指定释放对象的旋转值</para>
        /// </param>
        /// <param name="localScale">
        /// <para>输入参数说明：指定释放对象的大小</para>
        /// </param>
        /// <returns></returns>
        public GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
        {
#if UNITY_EDITOR
            if (!dictionary.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

                return null;
            }
#endif

            return dictionary[prefab].PreparedObject(position, rotation, localScale);
        }


        // public void ReturnPool(GameObject prefab, GameObject clone)
        // {
        //     dictionary[prefab].Return(clone);
        // }

        public void EnemyReturnPool(EnemyGeneratedData clone)
        {
            enemyPools[clone.name].EnemyReturn(clone);
        }

    }
}