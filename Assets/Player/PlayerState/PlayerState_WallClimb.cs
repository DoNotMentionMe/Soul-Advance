using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_WallClimb : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
            ctler.WallClimb();
            animManager.CrossFade(AnimName.WallClimb, 0f);
            //计算爬上去后位置
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (animManager.IsAnimEnded(AnimName.WallClimb))
            {
                //位置重置到爬上去的位置
                FSM.SwitchState(typeof(PlayerState_HasWallClimbed));
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
