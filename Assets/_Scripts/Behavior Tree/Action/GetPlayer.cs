using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("获取场上的玩家对象")]
    public class GetPlayer : Action
    {
        [SerializeField] SharedPlayerFSM playerFSM;

        public override TaskStatus OnUpdate()
        {
            playerFSM.Value = PlayerFSM.Player;
            return TaskStatus.Success;
        }
    }
}
