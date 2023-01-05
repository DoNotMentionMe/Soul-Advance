using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class BOSSProperty : EnemyProperty
    {
        //public UnityEvent<int> On阶段改变 = new UnityEvent<int>();
        /// <summary>
        /// 行为树Action:UnityEventInvoke调用
        /// </summary>
        //[Button] public void InvokeOn阶段改变() { On阶段改变?.Invoke(当前阶段); Debug.Log($"阶段改变"); }
        [Button] public void 向行为树发送阶段改变信号1() { 当前阶段 = 1; mTree.SendEvent("阶段改变信号"); Debug.Log($"发送1阶段改变信号"); }
        [Button] public void 向行为树发送阶段改变信号2() { 当前阶段 = 2; mTree.SendEvent("阶段改变信号"); Debug.Log($"发送2阶段改变信号"); }
        [Button] public void 向行为树发送阶段改变信号3() { 当前阶段 = 3; mTree.SendEvent("阶段改变信号"); Debug.Log($"发送3阶段改变信号"); }
        [Space]
        [Header("BOSS多阶段血量分界线")]
        [OnValueChanged("OnValueChangedCallback")]
        [AllowNesting]
        [SerializeField] int 阶段数;
        [ShowNativeProperty] public int 当前阶段 { get; set; } = 1;//由行为树的“当前阶段”变量直接调用
        [SerializeField] List<float> 阶段分界线;

        public override void BeAttacked(int damage)
        {
            base.BeAttacked(damage);
            if (当前阶段 < 阶段数 && HP <= (MaxHP + ExtraHP) * 阶段分界线[当前阶段 - 1] / 100)
            {
                当前阶段++;
                //向行为树发送阶段改变信号
                mTree.SendEvent("阶段改变信号");
            }
        }
    }
}
