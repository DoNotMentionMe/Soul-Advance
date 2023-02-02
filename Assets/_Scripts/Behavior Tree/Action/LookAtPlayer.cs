using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("水平方向面向玩家")]
    public class LookAtPlayer : Action
    {
        [SerializeField] bool AutoGetPlayer = true;
        [SerializeField] SharedPlayerFSM playerFSM;
        [SerializeField] SharedTransform mTransform;

        private float direction;
        private Transform player;
        private bool playerNull = true;

        public override void OnStart()
        {
            if (AutoGetPlayer && playerNull)
            {
                player = PlayerFSM.Player.transform;
                playerNull = false;
            }
            else if (!AutoGetPlayer)
            {
                player = playerFSM.Value.transform;
            }

            direction = player.position.x - mTransform.Value.position.x;
        }

        public override TaskStatus OnUpdate()
        {
            if (mTransform.Value.localScale.x * direction < 0)
            {
                var Scale = mTransform.Value.localScale;
                Scale.x *= -1;
                mTransform.Value.localScale = Scale;
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            player = null;
            playerNull = true;
        }

    }
}
