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
        [Foldout("被击中表现设置")][SerializeField] float AnimFreezeFrameRange;
        [Foldout("被击中表现设置")][SerializeField] Vector2 CharacterShakeStrength;
        [Foldout("被击中表现设置")][SerializeField] float HitBackStartSpeed;
        [Foldout("被击中表现设置")][SerializeField] float HitBackDececleration;
        [Foldout("死亡表现设置")][SerializeField] GameObject 死亡爆炸特效;
        [Foldout("死亡表现设置")][SerializeField] float 屏幕震动幅度 = 1f;
        [Foldout("死亡表现设置")][SerializeField] float 屏幕震动频率 = 0.5f;
        [Foldout("组件")][SerializeField] BehaviorTree mBehaviorTree;
        [Foldout("组件")][SerializeField] apPortrait mApPortrait;
        [Foldout("组件")][SerializeField] Rigidbody2D mRigidbody;

        private float StartTime;
        private PlayerEffectPerformance playerEffect;
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
        }

        private void OnDestroy()
        {
            FreezeFrameing = null;
            HittedBacking = null;
            playerEffect = null;
        }

        public void DiedEffect()
        {
            PoolManager.Instance.Release(死亡爆炸特效, transform.position);
            ImpulseController.Instance.ProduceImpulse(transform.position, 屏幕震动幅度, 屏幕震动频率);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 被命中反馈，拖拽到EnemyProperty的BeHittedEvent中
        /// </summary>
        public void BeHittedEffect()
        {
            //TODO 这部分仅为测试用
            if (playerEffect == null)
            {
                playerEffect = PlayerFSM.Player.effect;
            }

            FreezeFrameing.Value = true;
            //敌人被命中反馈
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
            mApPortrait.transform.DOShakePosition(playerEffect.AttackHittedFreezeTime, CharacterShakeStrength);
            StartHittedBack();
        }

        private void StartHittedBack()
        {
            HittedBacking.Value = true;
            int direction = 0;
            if (playerEffect.transform.position.x >= transform.position.x)
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
