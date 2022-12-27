using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;
using DG.Tweening;

namespace Adv
{
    public class EnemyAnimManager : MonoBehaviour
    {
        [SerializeField] Trigger2D 攻击碰撞体;
        [SerializeField] apPortrait mApPortrait;

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
            //开启动画碰撞体
            攻击碰撞体.SetCollEnable(true);
        }

        public void AttackEnd()
        {
            攻击碰撞体.SetCollEnable(false);
        }

        #endregion
    }
}
