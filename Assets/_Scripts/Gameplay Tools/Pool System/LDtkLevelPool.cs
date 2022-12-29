using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class LDtkLevelPool
    {
        public LDtkLevel Prefab { get => prefab; set => prefab = value; }
        public string Key => string.Concat(prefab.name, "(Clone)");

        public int Size { get => size; set => size = value; }
        public int RuntimeSize => enemyQueue.Count;

        [SerializeField] LDtkLevel prefab;
        [SerializeField] int size = 1;

        Queue<LDtkLevel> enemyQueue;

        Transform parent;

        public void Initialize(Transform parent)
        {
            enemyQueue = new Queue<LDtkLevel>(0);
            this.parent = parent;
            for (var i = 0; i < size; i++)
            {
                enemyQueue.Enqueue(Copy());
            }

        }

        LDtkLevel Copy()
        {
            var copy = GameObject.Instantiate(prefab, parent);

            copy.onClear += LevelClear;
            copy.gameObject.SetActive(false);

            return copy;
        }

        LDtkLevel AvailableLevel()
        {
            LDtkLevel availableEnemy = null;
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

        public LDtkLevel PreparedLevel()
        {
            LDtkLevel preparedEnemy = AvailableLevel();

            preparedEnemy.gameObject.SetActive(true);

            return preparedEnemy;
        }

        public LDtkLevel PreparedLevel(Vector3 position)
        {
            LDtkLevel preparedEnemy = AvailableLevel();

            preparedEnemy.transform.position = position;
            preparedEnemy.gameObject.SetActive(true);

            return preparedEnemy;
        }

        public void LevelClear(LDtkLevel enemy)
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
