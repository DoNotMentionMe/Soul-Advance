using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    public class BOSSProperty : EnemyProperty
    {
        [Space]
        [Header("BOSS多阶段血量分界线")]
        [OnValueChanged("OnValueChangedCallback")]
        [AllowNesting]
        [SerializeField] int 阶段数;
        [ShowNativeProperty] public int 当前阶段 { get; set; }//由行为树的“当前阶段”变量直接调用
        [SerializeField] List<float> 阶段分界线;

        public override void BeAttacked(int damage)
        {
            base.BeAttacked(damage);
            if (当前阶段 < 阶段数 && HP <= (MaxHP + ExtraHP) * 阶段分界线[当前阶段 - 1] / 100)
            {
                当前阶段++;
            }
        }
    }
}
