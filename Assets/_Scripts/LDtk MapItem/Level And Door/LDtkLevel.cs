using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    /// <summary>
    /// 地块
    /// </summary>
    public class LDtkLevel : MonoBehaviour
    {
        public List<Door_LDtk> DoorList = new List<Door_LDtk>();
        public List<EnemyGenerate> GenerateList = new List<EnemyGenerate>();

        protected virtual void Awake()
        {
            DoorList = GetComponentsInChildren<Door_LDtk>().ToList();
            GenerateList = GetComponentsInChildren<EnemyGenerate>().ToList();
        }

        protected virtual void OnEnable()
        {

        }

        /// <summary>
        /// 停止生成敌人，清除当前敌人，清除Level
        /// </summary>
        public virtual void ClearLevel()
        {
            GenerateList.ForEach((generate) => { generate.Clear(); });
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 设置地块的位置，然后开始生成敌人
        /// </summary>
        public void SetLevelPositionAndStartGenerateEnemy(Vector2 position)
        {
            transform.position = position;
            GenerateList.ForEach((generate) => { generate.StartGenerateEnemy(); });
        }

        /// <summary>
        /// 获取一个出入口，并设置为已使用状态
        /// </summary>
        public Door_LDtk GetDoorWithType(LDtkDoorType doortype)
        {
            for (var i = 0; i < DoorList.Count; i++)
            {
                if (DoorList[i].DoorType == doortype)
                {
                    DoorList[i].HasBeUsed = true;
                    return DoorList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 返回未被使用的门的数列
        /// </summary>
        /// <returns></returns>
        // public Door_LDtk[] FindDoorsNotBeUsed()
        // {
        //     Door_LDtk[] list = new Door_LDtk[0];
        //     for (var i = 0; i < DoorList.Count; i++)
        //     {
        //         if (!DoorList[i].HasBeUsed)
        //         {
        //             Array.Resize<Door_LDtk>(ref list, list.Length + 1);
        //             list[list.Length - 1] = DoorList[i];
        //         }
        //     }
        //     return list;
        // }
    }
}
