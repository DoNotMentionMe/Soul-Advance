using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("检查玩家是否在背面")]
    public class IsThePlayerOnTheBack : Conditional
    {
        [SerializeField] bool CheckOnTheBack = true;
        [SerializeField] SharedPlayerFSM player;
        [SerializeField] SharedTransform mTransform;

        public override TaskStatus OnUpdate()
        {
            if (CheckOnTheBack)
                return IsPlayerOnTheBack();
            else
                return IsPlayerInTheFront();
        }

        private TaskStatus IsPlayerInTheFront()
        {
            if (mTransform.Value.position.x > player.Value.transform.position.x && mTransform.Value.localScale.x < 0
            || mTransform.Value.position.x < player.Value.transform.position.x && mTransform.Value.localScale.x > 0)
                return TaskStatus.Success;
            else
                return TaskStatus.Failure;
        }

        private TaskStatus IsPlayerOnTheBack()
        {
            if (mTransform.Value.position.x > player.Value.transform.position.x && mTransform.Value.localScale.x < 0
            || mTransform.Value.position.x < player.Value.transform.position.x && mTransform.Value.localScale.x > 0)
                return TaskStatus.Failure;
            else
                return TaskStatus.Success;
        }




    }
}
