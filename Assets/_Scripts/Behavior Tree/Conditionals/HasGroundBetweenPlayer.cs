using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("检测和玩家之间是否存在墙体")]
    public class HasGroundBetweenPlayer : Conditional
    {
        [SerializeField] bool checkHasGround = false;//检测和玩家之间有墙
        [SerializeField] SharedPlayerFSM player;
        [SerializeField] LayerMask checkLayers;//Player、Ground、OneWayPlatformer
        private const string Ground = "Ground";

        public override TaskStatus OnUpdate()
        {
            var direction = (player.Value.transform.position - transform.position).normalized;
            var distance = Vector2.Distance(player.Value.transform.position, transform.position);
            var results = new RaycastHit2D[1];
            var count = Physics2D.RaycastNonAlloc(transform.position, direction, results, distance, checkLayers);
            if (checkHasGround && count > 0 && results[0].collider.CompareTag(Ground))
                return TaskStatus.Success;
            else if (!checkHasGround && count > 0 && !results[0].collider.CompareTag(Ground))
                return TaskStatus.Success;
            else
                return TaskStatus.Failure;

        }

        // public override void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawLine(transform.position, player.Value.transform.position);
        // }
    }
}
