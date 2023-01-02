using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MoreMountains.Feedbacks;

namespace Adv
{
    public class PlayerEffectPerformance : MonoBehaviour
    {
        public float IntervalOfHittedEffect => intervalOfHittedEffect;
        public float AttackHittedFreezeTime => attackHittedFreezeTime;
        public float SecondFreezeTime => secondFreezeTime;

        [Foldout("攻击命中设置")][SerializeField] float attackHittedFreezeTime;//顿帧时间
        [Foldout("攻击命中设置")][SerializeField] float secondFreezeTime = 0.01f;//次级顿帧时间
        [Foldout("攻击命中设置")][SerializeField] float VelocityFreezeValue;//速度百分比
        [Foldout("攻击命中设置")][SerializeField] float AnimSpeedFreezeValue;//动画播放速度百分比
        [Foldout("攻击命中设置")][SerializeField] float intervalOfHittedEffect;//控制攻击命中特效播放间隔
        [Foldout("攻击命中设置")][SerializeField] GameObject HittedEffect;
        [Foldout("攻击命中设置")][SerializeField] MMF_Player AttackHittedFeedBacks;
        [SerializeField] GameObject 落地灰尘;
        //[SerializeField] AudioData AttackSound;
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
            animManager.CurrentAnimSpeedSlowDownForAWhile(AnimSpeedFreezeValue, AttackHittedFreezeTime, secondFreezeTime);
            playerController.FullControlVelocity(VelocityFreezeValue, AttackHittedFreezeTime, secondFreezeTime);
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
            StartCoroutine(TransformFollowAnother(obj, BeHittedObj));
        }

        #endregion

        #region 单一功能
        //public void PlayAttackSound() => AudioManager.Instance.PlayRandomSFX(AttackSound);
        public void Release落地灰尘() => PoolManager.Instance.Release(落地灰尘, mTransform.position, Quaternion.identity, transform.localScale);
        #endregion

        private Transform mTransform;
        private List<float> AttackHittedEffectList = new List<float>();//用来记录执行间隔和执行特效次数
        private Coroutine AttackHittedEffectCorotine;//攻击命中顺序执行协程
        private WaitForSecondsRealtime waitForIntervalHittedEffect;
        private WaitForSecondsRealtime waitForAttackHittedFreezeTime;
        private WaitForSecondsRealtime waitForSecondFreezeTime;

        private void Awake()
        {
            mTransform = transform;
            waitForIntervalHittedEffect = new WaitForSecondsRealtime(intervalOfHittedEffect);
            waitForAttackHittedFreezeTime = new WaitForSecondsRealtime(AttackHittedFreezeTime);
            waitForSecondFreezeTime = new WaitForSecondsRealtime(secondFreezeTime);
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
                float freezeTime;
                if (firstFreeze)
                {
                    firstFreeze = false;
                    freezeTime = AttackHittedEffectList[0];
                }
                else
                    freezeTime = secondFreezeTime;
                AttackHittedEffectList.RemoveAt(0);
                yield return StartCoroutine(AttackHittedEvent(freezeTime));
            }
            AttackHittedEffectCorotine = null;
        }
        /// <summary>
        /// 特效协程
        /// </summary>
        IEnumerator AttackHittedEvent(float FreezeTime)
        {
            AttackHittedFeedBacks.PlayFeedbacks();
            if (FreezeTime == AttackHittedFreezeTime)
            {
                //Debug.Log($"FirstAudioEffect,{Time.time}");
                yield return waitForAttackHittedFreezeTime;
                //Debug.Log($"FirstAudioEffectEnd,{Time.time}");
            }
            else
                yield return waitForSecondFreezeTime;
            yield return waitForIntervalHittedEffect;
        }

        IEnumerator TransformFollowAnother(GameObject follower, Collider2D another)
        {
            var followerTransform = follower.transform;
            var anotherGameObject = another.gameObject;
            while (follower.activeSelf && anotherGameObject.activeSelf)
            {
                followerTransform.position = another.bounds.center;
                yield return null;
            }
        }
    }
}
