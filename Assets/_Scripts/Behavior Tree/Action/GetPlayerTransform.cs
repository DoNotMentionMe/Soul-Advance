using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    public class GetPlayerTransform : Action
    {
        [SerializeField] SharedTransform playerTransform;

        public override TaskStatus OnUpdate()
        {
            playerTransform.Value = PlayerFSM.Player.transform;
            return TaskStatus.Success;
        }
    }
}
