using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

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
        [BoxGroup("Setting")][SerializeField] int 生成地图的地块数;
        [BoxGroup("Setting")][SerializeField] LDtkLevelEventChannel PlayerEnterLevel;
        [BoxGroup("Setting")][SerializeField] LDtkLevelEventChannel PlayerLeaveLevel;
        [Foldout("LaodList")][SerializeField] List<LDtkLevel_Entrance> EntranceList;
        [Foldout("LaodList")][SerializeField] List<LDtkLevel> ExitList;
        [Foldout("LaodList")][SerializeField] List<LDtkLevel> LeftRightList;
        private LDtkLevel_Entrance currentEntrance;
        private LDtkLevel currentExit;
        private Door_LDtk CurrentDoor;//记录已经生成的最右侧地图块的出口
        private List<LDtkLevel> currentLevels = new List<LDtkLevel>();//记录从左往右所有关卡
        private List<LDtkLevel> ShowingLevels = new List<LDtkLevel>();//记录从左往右所有关卡
        private List<LDtkLevel> PlayerLocallevels = new List<LDtkLevel>();//当前玩家的位置
        private int PlayerLocalIndex;//当前玩家所在currentLevels中的位置
        private int 当前需要生成地块数;
        /// <summary>
        /// 玩家位置变动时调用，激活玩家附近五个地图块
        /// </summary>
        private void ResetShowingLevels()
        {
            //TODO 同时只能执行一个，应该改为协程
            Debug.Log($"{PlayerLocallevels.Count}");
            //获取玩家所在位置index
            if (PlayerLocallevels.Count == 0) return;//等于0，说明执行了重新生成函数
            var playerLevel = PlayerLocallevels[PlayerLocallevels.Count - 1];//获取玩家进入地块
            for (var i = currentLevels.Count - 1; i >= 0; i--)
            {
                if (playerLevel == currentLevels[i])
                {
                    //找到玩家所在地图块位置，记录编号i
                    if (i == PlayerLocalIndex) return;//位置没有变化，不用往下执行
                    PlayerLocalIndex = i;
                    break;
                }
            }
            Debug.Log($"重新生成地块");
            //检查是否需要生成新地图块
            if (currentLevels.Count - 1 < PlayerLocalIndex + 2)
            {
                //生成新地图块
                GenerateNewLevelInCurrentDoor();
                if (currentLevels.Count - 1 < PlayerLocalIndex + 2)
                {
                    GenerateNewLevelInCurrentDoor();
                }
            }
            //分别显示并加入ShowingLevels地图块PlayerLocalIndex-2、PlayerLocalIndex-1、PlayerLocalIndex、PlayerLocalIndex+1、PlayerLocalIndex+2，
            //如果PlayerLocalIndex-2、PlayerLocalIndex-1小于零就不进行操作
            //如果要显示地图块已经存在ShowingLevels就不进行操作
            //Debug.Log($"应该显示的地图块为{PlayerLocalIndex - 2}、{PlayerLocalIndex - 1}、{PlayerLocalIndex}、{PlayerLocalIndex + 1}、{PlayerLocalIndex + 2}");
            for (var i = -2; i < 3; i++)
            {
                if (PlayerLocalIndex + i < 0) continue;

                var level = currentLevels[PlayerLocalIndex + i];
                if (!level.IsShowing)
                {
                    level.ShowLevel();
                    ShowingLevels.Add(level);
                }
            }
            //将ShowingLevels中的所有level和刚显示的五个地图块作比较，多余的level隐藏
            for (var i = ShowingLevels.Count - 1; i >= 0; i--)
            {
                //Debug.Log($"{i}");
                var showingLevel = ShowingLevels[i];
                for (var j = -2; j < 3; j++)
                {
                    if (PlayerLocalIndex + j < 0) continue;
                    if (showingLevel == currentLevels[PlayerLocalIndex + j])
                    {
                        break;
                    }

                    if (j == 2)
                    {
                        showingLevel.HideLevel();
                        ShowingLevels.Remove(showingLevel);
                    }
                }
            }
        }

        [Button]
        public void LoadAllLDtkLevel()
        {
            EntranceList.Clear();
            ExitList.Clear();
            LeftRightList.Clear();
            EntranceList = Resources.LoadAll<LDtkLevel_Entrance>(EntrancePath).ToList();
            ExitList = Resources.LoadAll<LDtkLevel>(ExitPath).ToList();
            LeftRightList = Resources.LoadAll<LDtkLevel>(LeftRightPath).ToList();
        }

        protected override void Awake()
        {
            base.Awake();
            当前需要生成地块数 = 生成地图的地块数;
            PlayerEnterLevel.AddListener(PlayerEnterLevelListener);
            PlayerLeaveLevel.AddListener(PlayerLeaveLevelListener);
        }

        private void OnDestroy()
        {
            PlayerEnterLevel.RemoveListenner(PlayerEnterLevelListener);
            PlayerLeaveLevel.RemoveListenner(PlayerLeaveLevelListener);
        }

        private void Start()
        {
            GenerateTopThreeLevel();
        }

        public void ReGenerateMap()
        {
            foreach (var level in currentLevels)
            {
                level.ClearLevel();
            }

            currentLevels.Clear();
            ShowingLevels.Clear();
            PlayerLocallevels.Clear();

            GenerateTopThreeLevel();
        }

        /// <summary>
        /// 生成前三个地图块
        /// </summary>
        private void GenerateTopThreeLevel()
        {
            //当前需要生成地块数 = 生成地图的地块数;
            CurrentDoor = CreateEntrance();
            CurrentDoor = CreateMapHorizontal(CurrentDoor);
            CurrentDoor = CreateMapHorizontal(CurrentDoor);
            Debug.Log($"生成3个地图块完成");
        }

        /// <summary>
        /// 在最右边生成一个新地图块
        /// </summary>
        private void GenerateNewLevelInCurrentDoor()
        {
            CurrentDoor = CreateMapHorizontal(CurrentDoor);
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
            ShowingLevels.Add(nextLevel);
            currentLevels.Add(nextLevel);
            当前需要生成地块数--;
            return nextLevel.GetDoorWithType(LDtkDoorType.Right);
        }

        private Door_LDtk CreateEntrance()
        {
            var index = Random.Range(0, EntranceList.Count);
            currentEntrance = EntranceList.RandomReleaseEntrance(Vector3.zero);
            PlayerLocallevels.Add(currentEntrance);
            ShowingLevels.Add(currentEntrance);
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

        private void PlayerEnterLevelListener(LDtkLevel level)
        {
            if (!PlayerLocallevels.Contains(level))
                PlayerLocallevels.Add(level);
            //TODO 显示玩家所在关卡附近的关卡，隐藏其他关卡
            ResetShowingLevels();
        }

        private void PlayerLeaveLevelListener(LDtkLevel level)
        {
            if (PlayerLocallevels.Contains(level))
                PlayerLocallevels.Remove(level);
            //TODO 显示玩家所在关卡附近的关卡，隐藏其他关卡
            //ResetShowingLevels();
        }
    }

    public static class ListLDtkLevelExtensions
    {
        public static LDtkLevel RandomReleaseLevel(this List<LDtkLevel> list)
        {
            var index = Random.Range(0, list.Count);
            return PoolManager.Instance.ReleaseLDtkLevel(list[index]);
        }

        public static LDtkLevel_Entrance RandomReleaseEntrance(this List<LDtkLevel_Entrance> list, Vector3 position)
        {
            var index = Random.Range(0, list.Count);
            return (LDtkLevel_Entrance)(PoolManager.Instance.ReleaseLDtkLevel(list[index], position));
        }
    }
}
