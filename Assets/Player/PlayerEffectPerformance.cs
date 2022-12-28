using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Adv
{
    public class PlayerEffectPerformance : MonoBehaviour
    {
        public float AttackHittedFreezeTime => attackHittedFreezeTime;

        public WaitForSecondsRealtime waitForAttackHittedFreezeTime;
        public WaitForSecondsRealtime waitForSecondFreezeTime;

        [Foldout("攻击命中设置")][SerializeField] float attackHittedFreezeTime;//顿帧时间
        [Foldout("攻击命中设置")][SerializeField] float SecondFreezeTime = 0.01f;//顿帧时间
        [Foldout("攻击命中设置")][SerializeField] float VelocityFreezeValue;//速度百分比
        [Foldout("攻击命中设置")][SerializeField] float AnimSpeedFreezeValue;//动画播放速度百分比
        [Foldout("攻击命中设置")][SerializeField] float intervalOfHittedEffect;//控制攻击命中特效播放间隔
        [Foldout("攻击命中设置")][SerializeField] float 攻击命中震幅 = 0.2f;
        [Foldout("攻击命中设置")][SerializeField] GameObject HittedEffect;
        [Foldout("攻击命中设置")][SerializeField] AudioData AttackHittedSound;
        [SerializeField] GameObject 落地灰尘;
        [SerializeField] AudioData AttackSound;
        [Foldout("组件")][SerializeField] PlayerAnimManager animManager;
        [Foldout("组件")][SerializeField] PlayerController playerController;

        #region 事件功能
        /// <summary>
        /// 攻击开始事件
        /// </summary>
        public void AttackEffect()
        {
            //ImpulseController.Instance.ProduceImpulse(mTransform.position, 0.2f, 1);
            //手部尾迹.enabled = true;
            //ReleaseAttackShadowCor = StartCoroutine(nameof(ReleaseAttackShadow));

        }

        public void AttackEndEffect()
        {
            //手部尾迹SetFalse.Restart();
            //StopCoroutine(ReleaseAttackShadowCor);
        }
        /// <summary>
        /// 攻击命中特效，同时调用顺序执行
        /// 拖拽到攻击碰撞体执行
        /// </summary>
        public void AttackHittedEffect()
        {
            AttackHittedEffectList.Add(AttackHittedFreezeTime);
            if (AttackHittedEffectCorotine == null)
            {
                AttackHittedEffectCorotine = StartCoroutine(ExecuteAttackHittedEvent());
            }
            animManager.CurrentAnimSpeedSlowDownForAWhile(AnimSpeedFreezeValue, AttackHittedFreezeTime, SecondFreezeTime);
            playerController.FullControlVelocity(VelocityFreezeValue, AttackHittedFreezeTime, SecondFreezeTime);
        }

        /// <summary>
        /// 攻击命中特效，同时调用顺序执行
        /// 拖拽到攻击碰撞体执行
        /// </summary>
        public void AttackHittedEffectWithColl(Collider2D BeHittedObj)
        {
            //计算碰撞体的中心点
            var point = BeHittedObj.bounds.center;
            var randomRota = Quaternion.Euler(0, 0, Random.Range(0, 360f));
            var obj = PoolManager.Instance.Release(HittedEffect, point, randomRota);
            obj.transform.parent = BeHittedObj.transform;
        }

        #endregion

        #region 单一功能
        public void PlayAttackSound() => AudioManager.Instance.PlayRandomSFX(AttackSound);
        public void Release落地灰尘() => PoolManager.Instance.Release(落地灰尘, mTransform.position);
        #endregion

        private Transform mTransform;
        private List<float> AttackHittedEffectList = new List<float>();//用来记录执行间隔和执行特效次数
        private Coroutine AttackHittedEffectCorotine;//攻击命中顺序执行协程

        private void Awake()
        {
            mTransform = transform;
            waitForAttackHittedFreezeTime = new WaitForSecondsRealtime(AttackHittedFreezeTime + intervalOfHittedEffect);
            waitForSecondFreezeTime = new WaitForSecondsRealtime(SecondFreezeTime + intervalOfHittedEffect);
        }

        private void OnDestroy()
        {
            mTransform = null;
        }

        /// <summary>
        /// 顺序执行特效协程
        /// </summary>
        IEnumerator ExecuteAttackHittedEvent()
        {
            bool firstFreeze = true;
            while (AttackHittedEffectList.Count > 0)
            {
                var freezeTime = AttackHittedEffectList[0];
                AttackHittedEffectList.RemoveAt(0);
                if (firstFreeze)
                    firstFreeze = false;
                else
                    freezeTime = SecondFreezeTime;
                yield return StartCoroutine(AttackHittedEvent(freezeTime));
            }
            AttackHittedEffectCorotine = null;
        }
        /// <summary>
        /// 特效协程
        /// </summary>
        IEnumerator AttackHittedEvent(float FreezeTime)
        {
            AudioManager.Instance.PlayRandomSFX(AttackHittedSound);
            ImpulseController.Instance.ProduceImpulse(mTransform.position, 攻击命中震幅, 0.7f);
            if (FreezeTime == AttackHittedFreezeTime)
                yield return waitForAttackHittedFreezeTime;
            else
                yield return waitForSecondFreezeTime;
        }
    }
}
