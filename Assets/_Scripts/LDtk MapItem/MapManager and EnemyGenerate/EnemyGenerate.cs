using System;
using System.Net.Http.Headers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LDtkUnity;
using NaughtyAttributes;

namespace Adv
{
    /// <summary>
    /// 敌人生成区域
    /// 功能：自动生成敌人
    /// </summary>
    public class EnemyGenerate : MonoBehaviour, ILDtkImportedFields
    {
        [SerializeField] bool AutoGenerateEnemyWhenEnemyDied = false;
        [SerializeField] float 敌人生成间隔;//时间间隔
        //[SerializeField] List<EnemyGeneratedData> 该区域敌人生成列表 = new List<EnemyGeneratedData>();
        [SerializeField] EnemyGeneratedData 生成对象;
        [SerializeField] float Distance敌人生成距离;

        private List<EnemyGeneratedData> 该区域当前敌人 = new List<EnemyGeneratedData>();
        private bool CanGenerate = false;

        /// <summary>
        /// 重置左右两点的坐标位置并开始开始生成敌人
        /// </summary>
        [Button]
        public void StartGenerateEnemy()
        {
            //Debug.Log($"开始生成敌人，总数{可生成敌人总数},当前数{当前可生成数}");
            CanGenerate = true;
            // var halfDistance = 可生成敌人总数 / 2;
            // left = transform.position - Vector3.right * halfDistance;
            // right = transform.position + Vector3.right * halfDistance;
            ResetPos生成位置X值列表();
            生成敌人协程 = StartCoroutine(GenerateEnemy());
        }

        /// <summary>
        /// 当前可生成数是否满足敌人占用，满足则扣除占用生成敌人并返回真
        /// </summary>
        /// <param name="occupation"></param>
        /// <returns></returns>
        private bool CanGenerateOnThisRegion(EnemyGeneratedData EnemyPrefab)
        {
            //if (当前可生成数 >= EnemyPrefab.Occupation)
            if (当前可生成数 >= 1)
            {
                //当前可生成数 -= EnemyPrefab.Occupation;
                当前可生成数 -= 1;
                GenerateEnemy(EnemyPrefab);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 在left和right之间随机位置生成敌人
        /// </summary>
        /// <param name="EnemyPrefab"></param>
        private void GenerateEnemy(EnemyGeneratedData EnemyPrefab)
        {
            //var x = Random.Range(left.x, right.x);
            var x = Pos生成位置X值列表[(int)(可生成敌人总数 - 当前可生成数 - 1)];
            var y = transform.position.y + EnemyPrefab.GeneratedOffsetY;
            var generatePos = Vector2.right * x + Vector2.up * y;
            var newEnemy = PoolManager.Instance.ReleaseEnemy(EnemyPrefab, generatePos);
            var Scale = newEnemy.transform.localScale;
            Scale.x = 生成敌人转向;
            newEnemy.transform.localScale = Scale;
            该区域当前敌人.Add(newEnemy);
            newEnemy.GetRegion(this);
        }
        /// <summary>
        /// 敌人被关闭（死亡或地图清空）时调用，释放占用，并开始生成敌人
        /// </summary>
        /// <param name="occupation"></param>
        public void ReleaseOccupation(EnemyGeneratedData data)
        {
            该区域当前敌人.Remove(data);
            //当前可生成数 += data.Occupation;
            当前可生成数 += 1;
            //Debug.Log($"------释放{data.Occupation}占用，当前占用{当前可生成数}");
            if (AutoGenerateEnemyWhenEnemyDied && CanGenerate && 生成敌人协程 == null)
                生成敌人协程 = StartCoroutine(GenerateEnemy());
        }

        /// <summary>
        /// 显示被隐藏的敌人并开始生怪
        /// </summary>
        public void Show()
        {
            var enemyCount = 该区域当前敌人.Count;
            for (var i = enemyCount - 1; i >= 0; i--)
            {
                var enemy = 该区域当前敌人[i];
                enemy.gameObject.SetActive(true);
            }

            StartGenerateEnemy();
        }

        /// <summary>
        /// 只隐藏，不清除敌人
        /// </summary>
        public void Hide()
        {
            CanGenerate = false;
            if (生成敌人协程 != null)
            {
                StopCoroutine(生成敌人协程);
                生成敌人协程 = null;
            }

            var enemyCount = 该区域当前敌人.Count;
            for (var i = enemyCount - 1; i >= 0; i--)
            {
                var enemy = 该区域当前敌人[i];
                enemy.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 由LDtkLevel调用，用于清空地图
        /// </summary>
        public void Clear()
        {
            //Debug.Log($"----开始清空敌人生成区域");
            CanGenerate = false;
            if (生成敌人协程 != null)
            {
                StopCoroutine(生成敌人协程);
                生成敌人协程 = null;
            }

            var enemyCount = 该区域当前敌人.Count;
            for (var i = enemyCount - 1; i >= 0; i--)
            {
                var enemy = 该区域当前敌人[i];
                enemy.OnDied();
                //Debug.Log($"清空第{i}个");
            }
            //当前可生成数 = 可生成敌人总数;
            //Debug.Log($"----敌人生成区域清空完毕，区域总敌人数：{可生成敌人总数}，当前可生成数：{当前可生成数}");
        }

        [Header("Debug")]
        public float 可生成敌人总数;
        public int 生成敌人转向;
        [SerializeField] float 当前可生成数;
        // private Vector2 left;
        // private Vector2 right;
        private WaitForSeconds waitFor敌人生成间隔;
        private Coroutine 生成敌人协程;
        [SerializeField] List<float> Pos生成位置X值列表 = new List<float>();

        private void Awake()
        {
            waitFor敌人生成间隔 = new WaitForSeconds(敌人生成间隔);
            当前可生成数 = 可生成敌人总数;
            //var halfDistance = 可生成敌人总数 / 2;
            // left = transform.position - Vector3.right * halfDistance;
            // right = transform.position + Vector3.right * halfDistance;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            生成敌人协程 = null;
        }

        private void OnDestroy()
        {
            //StopAllCoroutines();
            if (生成敌人协程 != null)
                StopCoroutine(生成敌人协程);
            生成敌人协程 = null;
        }

        public void OnLDtkImportFields(LDtkFields fields)
        {
            var EnemyCount = fields.GetInt("EnemyCount");
            可生成敌人总数 = Mathf.Abs(EnemyCount);
            生成敌人转向 = Math.Sign(EnemyCount);
        }

        private void ResetPos生成位置X值列表()
        {
            Pos生成位置X值列表.Clear();
            var direction = 1;
            for (var i = 0; i < 可生成敌人总数; i++)
            {
                var offset = direction * i * Distance敌人生成距离;
                if (i > 0)
                    Pos生成位置X值列表.Add(Pos生成位置X值列表[i - 1] + offset);
                else
                    Pos生成位置X值列表.Add(transform.position.x + offset);
                direction *= -1;
            }
        }

        IEnumerator GenerateEnemy()
        {
            do
            {
                yield return waitFor敌人生成间隔;
            } while (CanGenerateOnThisRegion(生成对象));
            生成敌人协程 = null;
            //var count = (int)(可生成敌人总数 / 该区域敌人生成列表[0].Occupation);
            //Debug.Log($"生成结束，当前数{当前可生成数}，应存在{count}，实际存在{该区域当前敌人.Count}");
        }
    }

}
