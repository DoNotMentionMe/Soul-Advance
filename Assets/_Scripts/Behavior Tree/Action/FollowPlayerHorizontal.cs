using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    public class FollowPlayerHorizontal : Action
    {
        [SerializeField] SharedTrigger2D 视野;
        [SerializeField] SharedRigidbody2D mRigidbody;
        [SerializeField] SharedTransform mTransform;
        [SerializeField] float FollowSpeed;
        private Transform player;
        private int direction;

        public override void OnStart()
        {
            if (player == null)
                player = 视野.Value.LastEnterTransform;
        }

        public override TaskStatus OnUpdate()
        {
            SetVeclocityX();
            LookAtPlayer();
            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            player = null;
        }

        private void SetVeclocityX()
        {
            var velocity = mRigidbody.Value.velocity;
            velocity.x = mTransform.Value.localScale.x * FollowSpeed;
            velocity.y = 0;
            mRigidbody.Value.velocity = velocity;
        }

        private void LookAtPlayer()
        {
            if (mTransform.Value.position.x > player.position.x + 0.1f)
            {
                var localScale = mTransform.Value.localScale;
                localScale.x = -1;
                mTransform.Value.localScale = localScale;
            }
            else if (mTransform.Value.position.x < player.position.x - 0.1f)
            {
                var localScale = mTransform.Value.localScale;
                localScale.x = 1;
                mTransform.Value.localScale = localScale;
            }
        }
    }
}
