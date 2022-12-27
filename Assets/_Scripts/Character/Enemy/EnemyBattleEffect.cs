using UnityEngine;
using BehaviorDesigner.Runtime;
using AnyPortrait;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;

namespace Adv
{
    public class EnemyBattleEffect : MonoBehaviour
    {
        [Foldout("顿帧设置")][SerializeField] float FreezeFrameTime;//顿帧时间
        [Foldout("顿帧设置")][SerializeField] float PlayerAnimFreezeFrameRange;
        [Foldout("顿帧设置")][SerializeField] float PlayerVelocityFreezeFrameRange;
        [Foldout("顿帧设置")][SerializeField] float AnimFreezeFrameRange;
        [Foldout("被击中表现设置/震动")][SerializeField] Vector2 ShakeStrength;
        [Foldout("被击中表现设置/被击退")][SerializeField] float HitBackStartSpeed;
        [Foldout("被击中表现设置/被击退")][SerializeField] float HitBackDececleration;
        [Foldout("组件")][SerializeField] BehaviorTree mBehaviorTree;
        [Foldout("组件")][SerializeField] apPortrait mApPortrait;
        [Foldout("组件")][SerializeField] Rigidbody2D mRigidbody;

        private float StartTime;
        private PlayerFSM playerFSM;
        private SharedBool FreezeFrameing;//敌人行为树必带变量
        private SharedBool HittedBacking;//敌人行为树必带变量
        private Coroutine HittedBackCoroutine;


        private void Start()
        {
            FreezeFrameing = (SharedBool)mBehaviorTree.GetVariable("FreezeFrameing");
            HittedBacking = (SharedBool)mBehaviorTree.GetVariable("HittedBacking");
            // playerAnim = PlayerFSM.Player.animManager;
            // playerController = PlayerFSM.Player.ctler;
            // playerEffect = PlayerFSM.Player.effect;
        }

        private void OnEnable()
        {
            //需要先生成玩家，在生成敌人
            playerFSM = PlayerFSM.Player;
        }

        private void OnDestroy()
        {
            FreezeFrameing = null;
            HittedBacking = null;
            playerFSM = null;
        }

        /// <summary>
        /// 被命中反馈，拖拽到EnemyProperty的BeHittedEvent中
        /// </summary>
        public void BeHittedEffect()
        {
            //TODO 这部分仅为测试用
            if (playerFSM == null)
            {
                playerFSM = PlayerFSM.Player;
            }

            FreezeFrameing.Value = true;
            //玩家命中反馈
            playerFSM.animManager.CurrentAnimSpeedSlowDownForAWhile(PlayerAnimFreezeFrameRange, FreezeFrameTime);
            playerFSM.ctler.FullControlVelocity(PlayerVelocityFreezeFrameRange, FreezeFrameTime);
            playerFSM.effect.AttackHittedEffect(FreezeFrameTime);
            //敌人被命中反馈
            StartTime = Time.time;
            mApPortrait.SetAnimationSpeed(AnimFreezeFrameRange);
            mApPortrait.SetControlParamInt("Hitted", 1);
            mRigidbody.velocity = Vector2.zero;
            DOVirtual.DelayedCall(0.13f, () => { mApPortrait.SetControlParamInt("Hitted", 0); });
            DOVirtual.DelayedCall(FreezeFrameTime, () =>
            {
                mApPortrait.SetAnimationSpeed(1);
                FreezeFrameing.Value = false;
            });
            mApPortrait.transform.DOShakePosition(FreezeFrameTime, ShakeStrength);
            StartHittedBack();
        }

        private void StartHittedBack()
        {
            HittedBacking.Value = true;
            int direction = 0;
            if (playerFSM.transform.position.x >= transform.position.x)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            if (HittedBackCoroutine != null)
                StopCoroutine(HittedBackCoroutine);
            HittedBackCoroutine = StartCoroutine(HittedBack(direction));
        }

        IEnumerator HittedBack(int direction)
        {
            var startSpeed = HitBackStartSpeed;
            while (startSpeed > 0)
            {
                transform.position += Vector3.right * direction * startSpeed * Time.deltaTime;
                yield return null;
                startSpeed -= HitBackDececleration * Time.deltaTime;
            }
            HittedBacking.Value = false;
            HittedBackCoroutine = null;
        }

    }
}
