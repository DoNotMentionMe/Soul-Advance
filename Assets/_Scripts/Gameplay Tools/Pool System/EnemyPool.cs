using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    /// <summary>
    /// 需求：需要大量取出敌人并获取EnemyGeneratedData脚本，需要直接生成、储存、返回EnemyGeneratedData脚本，所以需要专属对象池
    /// </summary>
    [System.Serializable]
    public class EnemyPool
    {
        public EnemyGeneratedData Prefab { get => prefab; set => prefab = value; }
        public string Key => string.Concat(prefab.name, "(Clone)");

        public int Size { get => size; set => size = value; }
        public int RuntimeSize => enemyQueue.Count;

        [SerializeField] EnemyGeneratedData prefab;
        [SerializeField] int size = 1;

        Queue<EnemyGeneratedData> enemyQueue;

        Transform parent;

        public void Initialize(Transform parent)
        {
            enemyQueue = new Queue<EnemyGeneratedData>(0);
            this.parent = parent;
            for (var i = 0; i < size; i++)
            {
                enemyQueue.Enqueue(Copy());
            }

        }

        EnemyGeneratedData Copy()
        {
            var copy = GameObject.Instantiate(prefab, parent);

            copy.onDied += EnemyReturn;
            copy.gameObject.SetActive(false);

            return copy;
        }

        EnemyGeneratedData AvailableEnemy()
        {
            EnemyGeneratedData availableEnemy = null;
            if (enemyQueue.Count > 0 && enemyQueue.Peek().gameObject.activeSelf)
            {
                enemyQueue.Peek().gameObject.SetActive(false);
                availableEnemy = enemyQueue.Dequeue();
            }
            else if (enemyQueue.Count > 0)
            {
                availableEnemy = enemyQueue.Dequeue();
            }
            else
                availableEnemy = Copy();
            return availableEnemy;
        }

        public EnemyGeneratedData PreparedEnemy(Vector3 position)
        {
            EnemyGeneratedData preparedEnemy = AvailableEnemy();

            preparedEnemy.transform.position = position;
            preparedEnemy.gameObject.SetActive(true);

            return preparedEnemy;
        }

        public void EnemyReturn(EnemyGeneratedData enemy)
        {
            if (!enemyQueue.Contains(enemy))
            {
                //enemyList.Add(enemy);
                enemyQueue.Enqueue(enemy);

                //Debug.Log($"------放回对象池");
            }
        }
    }
}
