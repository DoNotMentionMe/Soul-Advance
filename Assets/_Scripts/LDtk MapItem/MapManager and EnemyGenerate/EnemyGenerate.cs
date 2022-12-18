using System.Net.Http.Headers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LDtkUnity;

namespace Adv
{
    /// <summary>
    /// 敌人生成区域
    /// 功能：自动生成敌人
    /// </summary>
    public class EnemyGenerate : MonoBehaviour, ILDtkImportedFields
    {
        [SerializeField] float 敌人生成间隔;
        [SerializeField] List<EnemyGeneratedData> 该区域敌人生成列表 = new List<EnemyGeneratedData>();

        private List<EnemyGeneratedData> 该区域当前敌人 = new List<EnemyGeneratedData>();
        private bool CanGenerate = false;

        /// <summary>
        /// 重置左右两点的坐标位置并开始开始生成敌人
        /// </summary>
        public void StartGenerateEnemy()
        {
            //Debug.Log($"开始生成敌人，总数{可生成敌人总数},当前数{当前可生成数}");
            CanGenerate = true;
            var halfDistance = 可生成敌人总数 / 2;
            left = mTransform.position - Vector3.right * halfDistance;
            right = mTransform.position + Vector3.right * halfDistance;

            生成敌人协程 = StartCoroutine(GenerateEnemy());
        }

        /// <summary>
        /// 当前可生成数是否满足敌人占用，满足则扣除占用生成敌人并返回真
        /// </summary>
        /// <param name="occupation"></param>
        /// <returns></returns>
        private bool CanGenerateOnThisRegion(EnemyGeneratedData EnemyPrefab)
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
        private void GenerateEnemy(EnemyGeneratedData EnemyPrefab)
        {
            var x = Random.Range(left.x, right.x);
            var y = mTransform.position.y + EnemyPrefab.GeneratedOffsetY;
            var generatePos = Vector2.right * x + Vector2.up * y;
            var newEnemy = PoolManager.Instance.ReleaseEnemy(EnemyPrefab, generatePos);
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
            当前可生成数 += data.Occupation;
            //Debug.Log($"------释放{data.Occupation}占用，当前占用{当前可生成数}");
            if (CanGenerate && 生成敌人协程 == null)
                生成敌人协程 = StartCoroutine(GenerateEnemy());
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
                该区域当前敌人[i].gameObject.SetActive(false);//Data设为false会调用ReleaseOccupation移出List
                //Debug.Log($"清空第{i}个");
            }
            //当前可生成数 = 可生成敌人总数;
            //Debug.Log($"----敌人生成区域清空完毕，区域总敌人数：{可生成敌人总数}，当前可生成数：{当前可生成数}");
        }


        public float 可生成敌人总数;
        [SerializeField] float 当前可生成数;
        private Transform mTransform;
        private Vector2 left;
        private Vector2 right;
        private WaitForSeconds waitFor敌人生成间隔;
        private Coroutine 生成敌人协程;

        private void Awake()
        {
            waitFor敌人生成间隔 = new WaitForSeconds(敌人生成间隔);
            mTransform = transform;
            当前可生成数 = 可生成敌人总数;
            var halfDistance = 可生成敌人总数 / 2;
            left = mTransform.position - Vector3.right * halfDistance;
            right = mTransform.position + Vector3.right * halfDistance;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            生成敌人协程 = null;
            mTransform = null;
        }

        public void OnLDtkImportFields(LDtkFields fields)
        {
            可生成敌人总数 = fields.GetInt("EnemyCount");
        }

        IEnumerator GenerateEnemy()
        {
            do
            {
                yield return waitFor敌人生成间隔;
            } while (CanGenerateOnThisRegion(该区域敌人生成列表[0]));
            生成敌人协程 = null;
            var count = (int)(可生成敌人总数 / 该区域敌人生成列表[0].Occupation);
            //Debug.Log($"生成结束，当前数{当前可生成数}，应存在{count}，实际存在{该区域当前敌人.Count}");
        }
    }

}
