using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Adv
{
    public class EnemyGenerator : PersistentSingleton<EnemyGenerator>
    {
        [SerializeField] EnemyGenerationRegionEventChannel GetEnemyGenerationRegion;
        [SerializeField] EnemyGeneratedData EnemyPrefab;

        //TODO 重新生成场景时需要先清空当前存储的敌人生成区域
        [SerializeField] List<EnemyGenerationRegion> currentSceneRegions = new List<EnemyGenerationRegion>();

        protected override void Awake()
        {
            base.Awake();

            GetEnemyGenerationRegion.AddListener((region) =>
            {
                if (!currentSceneRegions.Contains(region))
                    currentSceneRegions.Add(region);
            });
        }

        //完全随机生成
        public void GenerateEnemy(EnemyGeneratedData EnemyPrefab)
        {
            for (var i = 0; i < currentSceneRegions.Count; i++)
            {
                if (currentSceneRegions[i].CanGenerateOnThisRegion(EnemyPrefab))
                    break;
            }
        }
        [Header("测试")]
        [SerializeField] float GenerateInterval;
        public System.Action startGenerate = delegate { };
        private bool Generating;

        private void Start()
        {
            startGenerate += () =>
            {
                if (!Generating)
                    this.RunCoroutine(GenerateEnemyUntilNoRegion());
            };
            DOVirtual.DelayedCall(0.2f, () => { startGenerate.Invoke(); });

        }

        IEnumerator GenerateEnemyUntilNoRegion()
        {
            Generating = true;
            for (var i = 0; i < currentSceneRegions.Count; i++)
            {
                while (currentSceneRegions[i].CanGenerateOnThisRegion(EnemyPrefab))
                {
                    yield return new WaitForSeconds(GenerateInterval);
                }
            }
            Generating = false;
        }

    }
}
