using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using AnyPortrait;
using DG.Tweening;

namespace Adv
{
    public class EnemyBattleEffect : MonoBehaviour
    {
        [SerializeField] BehaviorTree mBehaviorTree;
        [SerializeField] float FreezeFrameTime;
        [SerializeField] float PlayerAnimFreezeFrameRange;
        [SerializeField] float PlayerVelocityFreezeFrameRange;
        [SerializeField] float AnimFreezeFrameRange;
        [SerializeField] Vector2 ShakeStrength;
        [SerializeField] apPortrait mApPortrait;
        [SerializeField] Rigidbody2D mRigidbody;

        private float StartTime;
        private PlayerAnimManager playerAnim;
        private PlayerController playerController;
        private PlayerEffectPerformance playerEffect;
        private SharedBool FreezeFrameing;


        private void Start()
        {
            FreezeFrameing = (SharedBool)mBehaviorTree.GetVariable("FreezeFrameing");
            playerAnim = PlayerFSM.Player.animManager;
            playerController = PlayerFSM.Player.ctler;
            playerEffect = PlayerFSM.Player.effect;
        }

        /// <summary>
        /// 被命中反馈，拖拽到EnemyProperty的BeHittedEvent中
        /// </summary>
        public void BeHittedEffect()
        {
            FreezeFrameing.Value = true;
            //玩家命中反馈
            playerAnim.CurrentAnimSpeedSlowDownForAWhile(PlayerAnimFreezeFrameRange, FreezeFrameTime);
            playerController.FullControlVelocity(PlayerVelocityFreezeFrameRange, FreezeFrameTime);
            playerEffect.AttackHittedEffect(FreezeFrameTime);
            //敌人被命中反馈
            StartTime = Time.time;
            mApPortrait.SetAnimationSpeed(AnimFreezeFrameRange);
            mRigidbody.velocity = Vector2.zero;
            DG.Tweening.DOVirtual.DelayedCall(FreezeFrameTime, () =>
            {
                mApPortrait.SetAnimationSpeed(1);
                FreezeFrameing.Value = false;
            });
            mApPortrait.transform.DOShakePosition(FreezeFrameTime, ShakeStrength);
        }

        private void OnDestroy()
        {
            FreezeFrameing = null;
            playerAnim = null;
            playerController = null;
            playerEffect = null;
        }
    }
}
