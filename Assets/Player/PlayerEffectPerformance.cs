using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Adv
{
    public class PlayerEffectPerformance : MonoBehaviour
    {
        [SerializeField] TrailRenderer 手部尾迹;
        [SerializeField] ParticleSystem 手部粒子效果;
        [SerializeField] GameObject 落地灰尘;
        [SerializeField] PlayerAttackShadow AttackShadow;
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
            手部粒子效果.Play();
            //ReleaseAttackShadowCor = StartCoroutine(nameof(ReleaseAttackShadow));

        }

        public void AttackEndEffect()
        {
            //手部尾迹SetFalse.Restart();
            //StopCoroutine(ReleaseAttackShadowCor);
        }
        /// <summary>
        /// 攻击命中事件
        /// 拖拽到AttackBox的Action调用
        /// </summary>
        public void AttackHittedEffect()
        {
            //动画播放变慢
            //DOVirtual.DelayedCall(0.1f, () => { animManager.CurrentAnimSpeedSlowDown(1); }).OnPlay(() => { animManager.CurrentAnimSpeedSlowDown(0.1f); });

            AudioManager.Instance.PlayRandomSFX(AttackHittedSound);
            ImpulseController.Instance.ProduceImpulse(mTransform.position, 0.2f, 1.2f);
            GameController.Instance.StartTimePause();

        }

        #endregion

        #region 单一功能
        public void PlayAttackSound() => AudioManager.Instance.PlayRandomSFX(AttackSound);
        public void Release落地灰尘() => PoolManager.Instance.Release(落地灰尘, mTransform.position);
        #endregion

        private Transform mTransform;
        private Tween 手部尾迹SetFalse;
        private WaitForSecondsRealtime waitForIntervalShadow;
        private Coroutine ReleaseAttackShadowCor;

        private void Awake()
        {
            mTransform = transform;
            //手部尾迹SetFalse = DOVirtual.DelayedCall(0.2f, () => { 手部尾迹.enabled = false; }).SetAutoKill(false);
            waitForIntervalShadow = new WaitForSecondsRealtime(ReleaseIntervalOfAttackShadow);
        }

        private void OnDestroy()
        {
            mTransform = null;
            //手部尾迹SetFalse.Kill();
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
