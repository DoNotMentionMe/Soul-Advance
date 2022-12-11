using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("降低玩家的动画速度和移速，并在若干时间后恢复")]
    public class PlayerFreezeFrame : Action
    {
        [SerializeField] SharedFloat FreezeFrameTime;
        [SerializeField] SharedFloat AnimFreezeFrameRange;
        [SerializeField] SharedFloat VelocityFreezeFrameRange;

        private PlayerAnimManager playerAnim;
        private PlayerController playerController;

        public override void OnAwake()
        {
            playerAnim = PlayerFSM.Player.animManager;
            playerController = PlayerFSM.Player.ctler;
        }

        public override TaskStatus OnUpdate()
        {
            playerAnim.CurrentAnimSpeedSlowDownForAWhile(AnimFreezeFrameRange.Value, FreezeFrameTime.Value);
            playerController.FullControlVelocity(VelocityFreezeFrameRange.Value, FreezeFrameTime.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            playerAnim = null;
            playerController = null;
        }
    }
}
