using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    /// <summary>
    /// 层级关系：LDtkMapGenerator->LDtkLevel->Door、EnemyGenerate
    /// 功能1：输入指定地块数生成一条水平方向的路径
    /// </summary>
    public class LDtkMapGenerator : PersistentSingleton<LDtkMapGenerator>
    {
        private const string EntrancePath = "Entrance";
        private const string ExitPath = "Exit";
        private const string LeftRightPath = "LeftRight";
        [SerializeField] int 生成地图的地块数;
        [SerializeField] List<LDtkLevel_Entrance> EntranceList;
        [SerializeField] List<LDtkLevel> ExitList;
        [SerializeField] List<LDtkLevel> LeftRightList;
        private LDtkLevel_Entrance currentEntrance;
        private LDtkLevel currentExit;
        private List<LDtkLevel> currentLevels = new List<LDtkLevel>();
        private int 当前需要生成地块数;

        protected override void Awake()
        {
            base.Awake();
            当前需要生成地块数 = 生成地图的地块数;
            EntranceList = Resources.LoadAll<LDtkLevel_Entrance>(EntrancePath).ToList();
            LeftRightList = Resources.LoadAll<LDtkLevel>(LeftRightPath).ToList();
            ExitList = Resources.LoadAll<LDtkLevel>(ExitPath).ToList();
        }

        private void Start()
        {

            GenerateHorizontalMap();

        }

        public void ReGenerateMap()
        {
            //Debug.Log($"开始清理地块");
            foreach (var level in currentLevels)
            {
                level.ClearLevel();
                //Debug.Log($"{name}");
            }
            //Debug.Log($"地块清理完毕");

            //Debug.Log($"清空地块存储列表");
            currentLevels.Clear();
            //Debug.Log($"地块存储列表清空完毕");

            //Debug.Log($"开始重新生成地图");
            GenerateHorizontalMap();
            //Debug.Log($"生成完毕");
        }

        private void GenerateHorizontalMap()
        {
            当前需要生成地块数 = 生成地图的地块数;
            var curDoor = CreateEntrance();
            while (当前需要生成地块数 > 1)
            {
                curDoor = CreateMapHorizontal(curDoor);
            }
            CreateExit(curDoor);
        }

        /// <summary>
        /// 从左往右路线：输入当前地块的右门，生成下一个地块，并返回下一个地块的右门
        /// </summary>
        private Door_LDtk CreateMapHorizontal(Door_LDtk curDoor)
        {
            Door_LDtk nextDoor;
            var nextLevel = LeftRightList.RandomReleaseLevel();
            nextDoor = nextLevel.GetDoorWithType(LDtkDoorType.Left);
            nextLevel.SetLevelPositionAndStartGenerateEnemy(nextDoor.levelPositionOffset + curDoor.transform.position);
            currentLevels.Add(nextLevel);
            当前需要生成地块数--;
            return nextLevel.GetDoorWithType(LDtkDoorType.Right);
        }

        private Door_LDtk CreateEntrance()
        {
            var index = Random.Range(0, EntranceList.Count);
            currentEntrance = EntranceList.RandomReleaseEntrance(Vector3.zero);
            currentLevels.Add(currentEntrance);
            当前需要生成地块数--;
            return currentEntrance.GetDoorWithType(LDtkDoorType.Right);
        }

        private void CreateExit(Door_LDtk curDoor)
        {
            Door_LDtk nextDoor;
            currentExit = ExitList.RandomReleaseLevel();
            nextDoor = currentExit.GetDoorWithType(LDtkDoorType.Left);
            currentExit.SetLevelPositionAndStartGenerateEnemy(nextDoor.levelPositionOffset + curDoor.transform.position);
            currentLevels.Add(currentExit);
            当前需要生成地块数--;
        }
    }

    public static class ListLDtkLevelExtensions
    {
        public static LDtkLevel RandomReleaseLevel(this List<LDtkLevel> list)
        {
            var index = Random.Range(0, list.Count);
            return PoolManager.Instance.ReleaseLDtkLevel(list[index]).GetComponent<LDtkLevel>();
        }

        public static LDtkLevel_Entrance RandomReleaseEntrance(this List<LDtkLevel_Entrance> list, Vector3 position)
        {
            var index = Random.Range(0, list.Count);
            return PoolManager.Instance.ReleaseLDtkLevel(list[index], position).GetComponent<LDtkLevel_Entrance>();
        }
    }
}
