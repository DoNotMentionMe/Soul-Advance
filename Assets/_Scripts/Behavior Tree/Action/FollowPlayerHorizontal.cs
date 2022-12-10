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
        [SerializeField] SharedBool IsFollowingPlayer;
        [SerializeField] SharedRigidbody2D mRigidbody;
        [SerializeField] SharedTransform mTransform;
        [SerializeField] float FollowSpeed;
        [SerializeField] float MaxFollowDistance;
        private Transform player;
        private int direction;
        private bool playerNull = true;

        public override void OnStart()
        {
            if (playerNull)
            {
                player = PlayerFSM.Player.transform;
                playerNull = false;
            }

            IsFollowingPlayer.Value = true;
        }

        public override TaskStatus OnUpdate()
        {
            LookAtPlayer();
            SetVeclocityX();
            //判定是否超出追击距离
            return CheckDistanceWithPlayer();
        }

        public override void OnReset()
        {
            player = null;
            playerNull = true;
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

        private TaskStatus CheckDistanceWithPlayer()
        {
            if (Mathf.Abs(mTransform.Value.position.x - player.position.x) >= MaxFollowDistance)
            {
                IsFollowingPlayer.Value = false;
                return TaskStatus.Failure;//退出追击状态
            }
            return TaskStatus.Running;
        }
    }
}
