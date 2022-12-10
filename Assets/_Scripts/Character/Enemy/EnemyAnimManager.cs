using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;

namespace Adv
{
    public class EnemyAnimManager : MonoBehaviour
    {
        [SerializeField] Trigger2D 攻击碰撞体;
        [SerializeField] apPortrait mApPortrait;

        private void Awake()
        {
            //初始化ApPortrait
            mApPortrait.Initialize();
        }

        #region 动画事件

        //动画事件
        public void AttackStart()
        {
            //开启动画碰撞体
            攻击碰撞体.SetCollEnable(true);
        }

        #endregion
    }
}
