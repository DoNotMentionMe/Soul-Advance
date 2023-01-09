using System;
using UnityEngine;
using BehaviorDesigner.Runtime;
using AnyPortrait;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine.Events;

namespace Adv
{
    public class EnemyBattleEffect : MonoBehaviour
    {
        [Tooltip("基于骨骼动画组件的顿帧，如果没有骨骼动画，只会对行为树变量进行修改")]
        [Foldout("被击中表现设置")][SerializeField] bool 顿帧 = true;
        [Foldout("被击中表现设置")][ShowIf("顿帧")][SerializeField] bool 顿帧减速 = true;
        [Tooltip("勾选则使用ApPortrait控制动画顿帧和闪烁")]
        [Foldout("被击中表现设置")][ShowIf("顿帧")][SerializeField] bool 使用骨骼动画 = true;
        [Foldout("被击中表现设置")][ShowIf("使用骨骼动画")][SerializeField] float AnimFreezeFrameRange;
        [Space]
        [Foldout("被击中表现设置")][SerializeField] Vector2 CharacterShakeStrength;
        [Foldout("被击中表现设置")][SerializeField] float FlashTime = 0.13f;
        [Space]
        [Foldout("被击中表现设置")][SerializeField] bool 可以被击退 = true;
        [Foldout("被击中表现设置")][ShowIf("可以被击退")][SerializeField] float HitBackStartSpeed_Front;//正面
        [Foldout("被击中表现设置")][ShowIf("可以被击退")][SerializeField] float HitBackStartSpeed_Back;//背面
        [Foldout("被击中表现设置")][ShowIf("可以被击退")][SerializeField] float HitBackDececleration;
        [Space]
        [Foldout("被击中表现设置")][SerializeField] Vector2 PushPlayerForce_Front;//正面
        [Foldout("被击中表现设置")][SerializeField] Vector2 PushPlayerForce_Back;//背面
        [Space]
        [Foldout("被击中表现设置")][SerializeField] UnityEvent OnBeHitted;
        [Foldout("死亡表现设置")][SerializeField] GameObject 死亡爆炸特效;
        [Foldout("死亡表现设置")][SerializeField] MMF_Player DiedFeedbacks;
        [Foldout("死亡表现设置")][SerializeField] UnityEvent OnDied;
        [Foldout("组件")][SerializeField] BehaviorTree mBehaviorTree;
        [Foldout("组件")][ShowIf("使用骨骼动画")][SerializeField] apPortrait mApPortrait;
        [Foldout("组件")][SerializeField] Transform 动画;
        [Foldout("组件")][ShowIf("可以被击退")][SerializeField] Rigidbody2D mRigidbody;

        private PlayerEffectPerformance playerEffect;
        private PlayerController playerController;
        private SharedBool FreezeFrameing;//敌人行为树必带变量
        private SharedBool HittedBacking;//敌人行为树必带变量
        private Coroutine HittedBackCoroutine;
        private Coroutine HittedFlashCoroutine;
        private Coroutine HittedFreezeTimeCoroutine;
        private WaitForSeconds waitForFlashTime;
        private WaitForSeconds waitForFreezeTime;

        private void Awake()
        {
            waitForFlashTime = new WaitForSeconds(FlashTime);
        }

        private void Start()
        {
            if (mBehaviorTree != null)
            {
                FreezeFrameing = (SharedBool)mBehaviorTree?.GetVariable("FreezeFrameing");
                HittedBacking = (SharedBool)mBehaviorTree?.GetVariable("HittedBacking");
            }
            else
            {
                FreezeFrameing = new SharedBool();
                FreezeFrameing.Value = false;
                HittedBacking = new SharedBool();
                HittedBacking.Value = false;
            }
        }

        private void OnEnable()
        {
            //需要先生成玩家，在生成敌人
            playerEffect = PlayerFSM.Player?.effect;
            playerController = PlayerFSM.Player?.ctler;
            if (playerEffect != null)
                waitForFreezeTime = new WaitForSeconds(playerEffect.AttackHittedFreezeTime);
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
            DiedFeedbacks?.PlayFeedbacks();
            OnDied?.Invoke();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 被命中反馈，拖拽到EnemyProperty的BeHittedEvent中
        /// </summary>
        public void BeHittedEffect()
        {
            //这部分仅为测试用
            //if (playerEffect == null)
            //{
            //    playerEffect = PlayerFSM.Player.effect;
            //    playerController = PlayerFSM.Player?.ctler;
            //}

            //顿帧
            if (顿帧)
            {
                if (顿帧减速)
                    mRigidbody.velocity = Vector2.zero;
                StartFlash();
                StartFreezeTime();
            }
            //左右震动
            动画.DOShakePosition(playerEffect.AttackHittedFreezeTime, CharacterShakeStrength);
            //被击退和推开玩家
            StartHittedBack();

            OnBeHitted?.Invoke();
        }

        private void StartHittedBack()
        {
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
            //pushForce.x *= -backDirection;
            StartCoroutine(WaitFreezeEnd(() =>
            {
                //被击退
                if (HittedBackCoroutine != null)
                    StopCoroutine(HittedBackCoroutine);
                if (可以被击退)
                {
                    HittedBacking.Value = true;
                    HittedBackCoroutine = StartCoroutine(HittedBack(backDirection, hitBackSpeed));
                }
                //推开玩家
                //playerController.GetAPush(pushForce);
                playerController.GetHittedBackForce(pushForce.x);
            }));

        }

        IEnumerator WaitFreezeEnd(Action onEnd)
        {
            while (FreezeFrameing.Value) { yield return null; }
            onEnd?.Invoke();
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

        private void StartFreezeTime()
        {
            if (HittedFreezeTimeCoroutine != null)
                StopCoroutine(HittedFreezeTimeCoroutine);
            HittedFreezeTimeCoroutine = StartCoroutine(nameof(FreezeTime));
        }

        IEnumerator FreezeTime()
        {
            FreezeFrameing.Value = true;
            if (使用骨骼动画)
                mApPortrait?.SetAnimationSpeed(AnimFreezeFrameRange);
            yield return waitForFreezeTime;
            if (使用骨骼动画)
                mApPortrait?.SetAnimationSpeed(1);
            FreezeFrameing.Value = false;

            HittedFreezeTimeCoroutine = null;
        }

        private void StartFlash()
        {
            if (HittedFlashCoroutine != null)
                StopCoroutine(HittedFlashCoroutine);
            HittedFlashCoroutine = StartCoroutine(nameof(Flash));
        }

        IEnumerator Flash()
        {
            if (使用骨骼动画)
                mApPortrait?.SetControlParamInt("Hitted", 1);
            yield return waitForFlashTime;
            if (使用骨骼动画)
                mApPortrait?.SetControlParamInt("Hitted", 0);

            HittedFlashCoroutine = null;
        }

    }
}
