using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PoolManager : PersistentSingleton<PoolManager>
    {
        //每次增加新的对象池数组都需要在Awake()中初始化，在OnDestroy()中检查是否超出预定数量
        //1
        [SerializeField] Pool[] Enemy;
        [SerializeField] Pool[] EnemyItem;
        [SerializeField] Pool[] PlayerItem;

        static Dictionary<GameObject, Pool> dictionary;

        protected override void Awake()
        {
            base.Awake();
            dictionary = new Dictionary<GameObject, Pool>();

            //2
            Initialize(Enemy);
            Initialize(EnemyItem);
            Initialize(PlayerItem);
        }
#if UNITY_EDITOR
        private void OnDestroy()
        {
            //3
            CheckPoolSize(Enemy);
            CheckPoolSize(EnemyItem);
            CheckPoolSize(PlayerItem);
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

        //         public GameObject Release(GameObject prefab, Vector2 positionOffset)
        //         {
        // #if UNITY_EDITOR
        //             if (!dictionary.ContainsKey(prefab))
        //             {
        //                 Debug.LogError("Pool Manager could NOT find prefab: " + prefab.name);

        //                 return null;
        //             }
        // #endif

        //             return dictionary[prefab].PreparedObject(positionOffset);
        //         }
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


        public void ReturnPool(GameObject prefab, GameObject clone)
        {
            dictionary[prefab].Return(clone);
        }

    }
}