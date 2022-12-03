using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_WallClimb : PlayerState
    {
        private float endTimer;
        private AnyPortrait.apAnimPlayData WallClimb;
        public override void Enter()
        {
            base.Enter();
            endTimer = 0;
            ctler.WallClimb();
            //anim.Play("WallClimb");
            WallClimb = apPortrait.CrossFade("WallClimb", 0f);
            //计算爬上去后位置
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();
            endTimer += Time.fixedDeltaTime;
            //if (endTimer >= 0.33f)//WallClimb动画时长
            if (WallClimb.PlaybackStatus == AnyPortrait.apAnimPlayData.AnimationPlaybackStatus.Ended)
            {
                //位置重置到爬上去的位置
                ctler.EndWallClimb();
                FSM.SwitchState(typeof(PlayerState_HasWallClimbed));
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
