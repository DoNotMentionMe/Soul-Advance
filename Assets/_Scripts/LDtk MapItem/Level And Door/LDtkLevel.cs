using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    /// <summary>
    /// 地块
    /// </summary>
    public class LDtkLevel : MonoBehaviour
    {
        public bool IsShowing => gameObject.activeSelf;
        public event UnityAction<LDtkLevel> onClear = delegate { };
        public List<Door_LDtk> DoorList = new List<Door_LDtk>();
        public List<EnemyGenerate> GenerateList = new List<EnemyGenerate>();
        public LDtkLevelEventChannel PlayerEnterLevel;//由SetPosprocessLevel配置
        public LDtkLevelEventChannel PlayerLeaveLevel;//由SetPosprocessLevel配置

        protected virtual void Awake()
        {
            DoorList = GetComponentsInChildren<Door_LDtk>().ToList();
            GenerateList = GetComponentsInChildren<EnemyGenerate>().ToList();
        }

        protected virtual void OnEnable()
        {

        }

        /// <summary>
        /// 显示被隐藏的关卡
        /// </summary>
        public virtual void ShowLevel()
        {
            gameObject.SetActive(true);
            foreach (var generate in GenerateList)
            {
                generate.Show();
            }
        }

        /// <summary>
        /// 隐藏关卡，不清除敌人
        /// </summary>
        public virtual void HideLevel()
        {
            foreach (var generate in GenerateList)
            {
                generate.Hide();
            }
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 停止生成敌人，清除当前敌人，清除Level
        /// </summary>
        public virtual void ClearLevel()
        {
            //Debug.Log($"--开始清空地块");
            foreach (var generate in GenerateList)
            {
                generate.Clear();
            }
            onClear?.Invoke(this);
            playerColls.Clear();
            LevelColls.Clear();
            IsPlayerEnter = false;
            IsLevelColl = false;
            gameObject.SetActive(false);
            //Debug.Log($"--地块清空完毕");
        }

        /// <summary>
        /// 设置地块的位置，然后开始生成敌人
        /// </summary>
        public void SetLevelPositionAndStartGenerateEnemy(Vector2 position)
        {
            transform.position = position;
            foreach (var generate in GenerateList)
            {
                generate.StartGenerateEnemy();
            }
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

        public bool IsPlayerEnter;
        public bool IsLevelColl;
        private HashSet<Collider2D> playerColls = new HashSet<Collider2D>();
        private HashSet<Collider2D> LevelColls = new HashSet<Collider2D>();
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (!playerColls.Contains(col))
                    playerColls.Add(col);
                if (!IsPlayerEnter && playerColls.Count > 0)
                {
                    IsPlayerEnter = true;
                    PlayerEnterLevel.Broadcast(this);
                }
            }
            else if (col.CompareTag("LDtkLevel"))
            {
                if (!LevelColls.Contains(col))
                    LevelColls.Add(col);
                if (!IsLevelColl && LevelColls.Count > 0)
                    IsLevelColl = true;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (playerColls.Contains(col))
                    playerColls.Remove(col);
                if (IsPlayerEnter && playerColls.Count == 0)
                {
                    IsPlayerEnter = false;
                    PlayerLeaveLevel.Broadcast(this);
                }
            }
            else if (col.CompareTag("LDtkLevel"))
            {
                if (LevelColls.Contains(col))
                    LevelColls.Remove(col);
                if (IsLevelColl && LevelColls.Count == 0)
                    IsLevelColl = false;
            }
        }
    }
}
