using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("持续性动作，降低敌人的动画速度，停止移速，并在若干时间后恢复，才结束动作")]
    public class EnemyFreezeFrame : Action
    {
        [SerializeField] SharedFloat FreezeFrameTime;
        [SerializeField] SharedFloat AnimFreezeFrameRange;
        [SerializeField] SharedApPortrait mApPortrait;
        [SerializeField] SharedRigidbody2D mRigidbody;

        private float StartTime;

        public override void OnStart()
        {
            StartTime = Time.time;
            mApPortrait.Value.SetAnimationSpeed(AnimFreezeFrameRange.Value);
            mRigidbody.Value.velocity = Vector2.zero;
            DG.Tweening.DOVirtual.DelayedCall(FreezeFrameTime.Value, () =>
            {
                mApPortrait.Value.SetAnimationSpeed(1);
            });
        }

        public override TaskStatus OnUpdate()
        {
            if (Time.time - StartTime > FreezeFrameTime.Value)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Running;
            }
        }
    }
}
