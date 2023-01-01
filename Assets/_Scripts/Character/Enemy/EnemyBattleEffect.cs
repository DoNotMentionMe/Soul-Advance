using UnityEngine;
using BehaviorDesigner.Runtime;
using AnyPortrait;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using MoreMountains.Feedbacks;

namespace Adv
{
    public class EnemyBattleEffect : MonoBehaviour
    {
        [Foldout("被击中表现设置")][SerializeField] float AnimFreezeFrameRange;
        [Foldout("被击中表现设置")][SerializeField] Vector2 CharacterShakeStrength;
        [Foldout("被击中表现设置")][SerializeField] float HitBackStartSpeed_Front;//正面
        [Foldout("被击中表现设置")][SerializeField] float HitBackStartSpeed_Back;//背面
        [Foldout("被击中表现设置")][SerializeField] float HitBackDececleration;
        [Foldout("被击中表现设置")][SerializeField] Vector2 PushPlayerForce_Front;//正面
        [Foldout("被击中表现设置")][SerializeField] Vector2 PushPlayerForce_Back;//背面
        [Foldout("死亡表现设置")][SerializeField] GameObject 死亡爆炸特效;
        [Foldout("死亡表现设置")][SerializeField] MMF_Player DiedFeedbacks;
        [Foldout("组件")][SerializeField] BehaviorTree mBehaviorTree;
        [Foldout("组件")][SerializeField] apPortrait mApPortrait;
        [Foldout("组件")][SerializeField] Rigidbody2D mRigidbody;

        private float StartTime;
        private PlayerEffectPerformance playerEffect;
        private PlayerController playerController;
        private SharedBool FreezeFrameing;//敌人行为树必带变量
        private SharedBool HittedBacking;//敌人行为树必带变量
        private Coroutine HittedBackCoroutine;


        private void Start()
        {
            FreezeFrameing = (SharedBool)mBehaviorTree.GetVariable("FreezeFrameing");
            HittedBacking = (SharedBool)mBehaviorTree.GetVariable("HittedBacking");
            // playerAnim = PlayerFSM.Player.animManager;
            // playerController = PlayerFSM.Player.ctler;
        }

        private void OnEnable()
        {
            //需要先生成玩家，在生成敌人
            playerEffect = PlayerFSM.Player?.effect;
            playerController = PlayerFSM.Player?.ctler;
        }

        private void OnDisable()
        {
            playerEffect = null;
            playerController = null;
        }

        private void OnDestroy()
        {
            FreezeFrameing = null;
            HittedBacking = null;
        }

        public void DiedEffect()
        {
            PoolManager.Instance.Release(死亡爆炸特效, transform.position);
            DiedFeedbacks.PlayFeedbacks();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 被命中反馈，拖拽到EnemyProperty的BeHittedEvent中
        /// </summary>
        public void BeHittedEffect()
        {
            //TODO 这部分仅为测试用
            //if (playerEffect == null)
            //{
            //    playerEffect = PlayerFSM.Player.effect;
            //    playerController = PlayerFSM.Player?.ctler;
            //}

            //顿帧
            FreezeFrameing.Value = true;
            StartTime = Time.time;
            mApPortrait.SetAnimationSpeed(AnimFreezeFrameRange);
            mApPortrait.SetControlParamInt("Hitted", 1);
            mRigidbody.velocity = Vector2.zero;
            DOVirtual.DelayedCall(0.13f, () => { mApPortrait.SetControlParamInt("Hitted", 0); });
            DOVirtual.DelayedCall(playerEffect.AttackHittedFreezeTime, () =>
            {
                mApPortrait.SetAnimationSpeed(1);
                FreezeFrameing.Value = false;
            });
            //左右震动
            mApPortrait.transform.DOShakePosition(playerEffect.AttackHittedFreezeTime, CharacterShakeStrength);
            //被击退和推开玩家
            StartHittedBack();
        }

        private void StartHittedBack()
        {
            HittedBacking.Value = true;
            int backDirection = 0;//被击退方向
            var pushForce = Vector2.zero;//推开玩家力
            var hitBackSpeed = 0f;
            if (playerEffect.transform.position.x >= transform.position.x)
            {
                backDirection = -1;
            }
            else
            {
                backDirection = 1;
            }
            if (transform.localScale.x == -backDirection)//正面
            {
                pushForce = PushPlayerForce_Front;
                hitBackSpeed = HitBackStartSpeed_Front;
            }
            else if (transform.localScale.x == backDirection)//背面
            {
                pushForce = PushPlayerForce_Back;
                hitBackSpeed = HitBackStartSpeed_Back;
            }
            pushForce.x *= -backDirection;
            //被击退
            if (HittedBackCoroutine != null)
                StopCoroutine(HittedBackCoroutine);
            HittedBackCoroutine = StartCoroutine(HittedBack(backDirection, hitBackSpeed));
            //推开玩家
            playerController.GetAPush(pushForce);
        }

        IEnumerator HittedBack(int direction, float hitBackSpeed)
        {
            mRigidbody.velocity = Vector2.right * hitBackSpeed * direction;
            //var startSpeed = hitBackSpeed;
            while (Mathf.Abs(mRigidbody.velocity.x) > 0)
            {
                //transform.position += Vector3.right * direction * startSpeed * Time.deltaTime;
                mRigidbody.velocity = Vector2.right * Mathf.MoveTowards(mRigidbody.velocity.x, 0, HitBackDececleration * Time.deltaTime);
                yield return null;
                //startSpeed -= HitBackDececleration * Time.deltaTime;
            }
            HittedBacking.Value = false;
            HittedBackCoroutine = null;
        }

    }
}
