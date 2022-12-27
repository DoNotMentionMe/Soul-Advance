using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Adv
{
    public class PlayerEffectPerformance : MonoBehaviour
    {
        public float IntervalOfHittedEffect => intervalOfHittedEffect;
        [SerializeField] float intervalOfHittedEffect;//控制攻击命中特效播放间隔
        [SerializeField] float 攻击命中震幅 = 0.2f;
        [SerializeField] GameObject 落地灰尘;
        [SerializeField] PlayerAttackShadow AttackShadow;//预制
        [SerializeField] float ReleaseIntervalOfAttackShadow;
        [SerializeField] AudioData AttackSound;
        [SerializeField] AudioData AttackHittedSound;
        [SerializeField] PlayerAnimManager animManager;

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
        /// 行为属Action：PlayerFreezeFrame调用
        /// </summary>
        public void AttackHittedEffect(float ControlTime)
        {
            if (AttackHittedEffectList.Count > 0)
                AttackHittedEffectList.Add(0.01f);
            else
                AttackHittedEffectList.Add(ControlTime);
            if (AttackHittedEffectCorotine == null)
            {
                AttackHittedEffectCorotine = StartCoroutine(ExecuteAttackHittedEvent());
            }
        }

        #endregion

        #region 单一功能
        public void PlayAttackSound() => AudioManager.Instance.PlayRandomSFX(AttackSound);
        public void Release落地灰尘() => PoolManager.Instance.Release(落地灰尘, mTransform.position);
        #endregion

        private Transform mTransform;
        private WaitForSecondsRealtime waitForIntervalShadow;
        private List<float> AttackHittedEffectList = new List<float>();//用来记录执行间隔和执行特效次数
        private Coroutine AttackHittedEffectCorotine;//攻击命中顺序执行协程

        private void Awake()
        {
            mTransform = transform;
            waitForIntervalShadow = new WaitForSecondsRealtime(ReleaseIntervalOfAttackShadow);
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
            while (AttackHittedEffectList.Count > 0)
            {
                var controltime = AttackHittedEffectList[0];
                AttackHittedEffectList.RemoveAt(0);
                yield return StartCoroutine(AttackHittedEvent(controltime));
            }
            AttackHittedEffectCorotine = null;
        }
        /// <summary>
        /// 特效协程
        /// </summary>
        IEnumerator AttackHittedEvent(float controlTime)
        {
            AudioManager.Instance.PlayRandomSFX(AttackHittedSound);
            ImpulseController.Instance.ProduceImpulse(mTransform.position, 攻击命中震幅, 0.7f);
            yield return new WaitForSeconds(controlTime + IntervalOfHittedEffect);
        }

        IEnumerator ReleaseAttackShadow()
        {
            while (true)
            {
                yield return waitForIntervalShadow;
                PoolManager.Instance.Release(AttackShadow.gameObject, mTransform.position, mTransform.localRotation, mTransform.localScale * 3);
            }
        }
    }
}
