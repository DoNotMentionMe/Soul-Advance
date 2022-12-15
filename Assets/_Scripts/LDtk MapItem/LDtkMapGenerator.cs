using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class LDtkMapGenerator : MonoBehaviour
    {
        // private const string EntrancePath = "Entrance";
        // private const string ExitPath = "Exit";
        // private const string LeftRightPath = "LeftRight";
        [SerializeField] List<LDtkLevel> EntranceList;
        [SerializeField] List<LDtkLevel> ExitList;
        [SerializeField] List<LDtkLevel> LeftRightList;
        private LDtkLevel currentEntrance;

        private void Awake()
        {
            // EntranceList = Resources.LoadAll<LDtkLevel>(EntrancePath);
            // LeftRightList = Resources.LoadAll<LDtkLevel>(LeftRightPath);
            // ExitList = Resources.LoadAll<LDtkLevel>(ExitPath);
        }

        private void Start()
        {
            currentEntrance = PoolManager.Instance.ReleaseLDtkLevel(EntranceList[0]).GetComponent<LDtkLevel>();
            currentEntrance.transform.position = Vector3.zero;
            if (currentEntrance.DoorList[0].DoorType == LDtkDoorType.Right)
            {
                var nextLevel = PoolManager.Instance.ReleaseLDtkLevel(LeftRightList[0]).GetComponent<LDtkLevel>();
                var nextLevelDoorLeft = nextLevel.GetDoorWithType(LDtkDoorType.Left);
                nextLevel.transform.position = (Vector3)(nextLevelDoorLeft.levelPositionOffset) + currentEntrance.DoorList[0].transform.position;
            }
        }
    }
}
