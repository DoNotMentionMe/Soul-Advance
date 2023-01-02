using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;
using DG.Tweening;
using UnityEngine.Events;

namespace Adv
{
    public class EnemyAnimManager : MonoBehaviour
    {
        [SerializeField] apPortrait mApPortrait;
        [SerializeField] UnityEvent OnAttackStart = new UnityEvent();
        [SerializeField] UnityEvent OnAttackEnd = new UnityEvent();

        private bool ControlAnimSpeeding;

        private void Awake()
        {
            //初始化ApPortrait
            mApPortrait.Initialize();
        }
        public void AnimSpeedSlowDownForAWhile(float speed, float controlTime)
        {
            if (ControlAnimSpeeding) return;
            ControlAnimSpeeding = true;
            mApPortrait.SetAnimationSpeed(speed);
            DOVirtual.DelayedCall(controlTime, () =>
            {
                mApPortrait.SetAnimationSpeed(1);
                ControlAnimSpeeding = false;
            });
        }


        #region 动画事件

        //动画事件
        public void AttackStart()
        {
            OnAttackStart?.Invoke();
        }

        public void AttackEnd()
        {
            OnAttackEnd?.Invoke();
        }

        #endregion
    }
}
