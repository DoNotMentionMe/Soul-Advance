using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom/Rigidbody2D")]
    [TaskDescription("以direction为横向方向设置速度")]
    public class MoveHorizontal : Action
    {
        private enum MoveModes
        {
            WithLocalScaleX,
            WithDirection,
        }
        [SerializeField] bool 倒转方向 = false;
        [SerializeField] MoveModes MoveMode;
        [SerializeField] SharedRigidbody2D mRigidbody;
        [SerializeField] SharedTransform mTransform;
        [SerializeField] SharedInt direction;
        [SerializeField] float Speed;

        public override void OnStart()
        {
            if (MoveMode == MoveModes.WithDirection)
            {
                var velocity = mRigidbody.Value.velocity;
                velocity.x = direction.Value * Speed;
                mRigidbody.Value.velocity = velocity;

                if (mTransform.Value.localScale.x * direction.Value < 0)
                {
                    var localScale = mTransform.Value.localScale;
                    localScale.x *= -1;
                    mTransform.Value.localScale = localScale;
                }
            }
            else
            {
                var velocity = mRigidbody.Value.velocity;
                if (倒转方向)
                    velocity.x = -mTransform.Value.localScale.x * Speed;
                else
                    velocity.x = mTransform.Value.localScale.x * Speed;
                mRigidbody.Value.velocity = velocity;
            }
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

    }
}
