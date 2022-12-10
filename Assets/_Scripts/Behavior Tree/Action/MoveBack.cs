using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("向背面减速度后退一段时间")]
    public class MoveBack : Action
    {
        [SerializeField] SharedTransform mTransform;
        [SerializeField] SharedRigidbody2D mRigidBody;
        [SerializeField] float StartSpeed;
        [SerializeField] float LifeTime;

        private float direction;
        private float startTime;

        public override void OnStart()
        {
            direction = Mathf.Sign(mTransform.Value.localScale.x);
            startTime = Time.time;
        }

        public override TaskStatus OnUpdate()
        {
            if (Time.time - startTime <= LifeTime)
            {
                return TaskStatus.Running;
            }
            else
                return TaskStatus.Success;
        }

        public override void OnFixedUpdate()
        {
            //减速
            var velocity = mRigidBody.Value.velocity;
            velocity.x = direction * Mathf.Lerp(StartSpeed, 0, (Time.time - startTime) / LifeTime);
            mRigidBody.Value.velocity = velocity;
        }
    }
}
