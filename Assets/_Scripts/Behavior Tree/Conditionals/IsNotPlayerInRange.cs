using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("检测玩家是否在范围内")]
    public class IsNotPlayerInRange : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("检测玩家不在范围内")]
        [SerializeField] bool CheckHasNotPlayer = true;//检测玩家不在范围内
        [SerializeField] SharedPlayerFSM player;
        [SerializeField] float MaxDistanceXWithPlayer;
        [SerializeField] float MaxDistanceYWithPlayer;
        [Space]
        [Header("自定义bool变量判定")]
        [BehaviorDesigner.Runtime.Tasks.Tooltip("是否同时检测自定义bool类型变量")]
        [SerializeField] bool CheckCustomBool = false;
        [SerializeField] SharedBool CustomBool;
        [SerializeField] SharedBool CustomBoolComparison;

        public override TaskStatus OnUpdate()
        {
            if (CheckHasNotPlayer &&
            (Mathf.Abs(player.Value.transform.position.x - transform.position.x) > MaxDistanceXWithPlayer
            || Mathf.Abs(player.Value.transform.position.y - transform.position.y) > MaxDistanceYWithPlayer))
            {
                if (!CheckCustomBool)
                    return TaskStatus.Success;
                else if (CustomBool.Value == CustomBoolComparison.Value)
                    return TaskStatus.Success;
                else
                    return TaskStatus.Failure;

            }
            else if (!CheckHasNotPlayer &&
            (Mathf.Abs(player.Value.transform.position.x - transform.position.x) < MaxDistanceXWithPlayer
            || Mathf.Abs(player.Value.transform.position.y - transform.position.y) < MaxDistanceYWithPlayer))
            {
                if (!CheckCustomBool)
                    return TaskStatus.Success;
                else if (CustomBool.Value == CustomBoolComparison.Value)
                    return TaskStatus.Success;
                else
                    return TaskStatus.Failure;
            }
            else
                return TaskStatus.Failure;
        }
    }
}
