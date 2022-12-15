using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class EnemyProperty : CharacterProperty, IBeAttacked
    {
        [Header("敌人组件")]
        [SerializeField] BehaviorTree mTree;
        [SerializeField] string 受伤事件 = "BeAttacked";
        [SerializeField] string 死亡事件 = "Died";
        [SerializeField] UnityEvent BeHittedEvent;//被命中时调用EnemyBattleEffect.BeHittedEffect

        public override void BeAttacked()
        {
            base.BeAttacked();
            if (HP <= 0)
            {
                BeHittedEvent.Invoke();
                mTree.SendEvent(死亡事件);
            }
            else
            {
                BeHittedEvent.Invoke();
                mTree.SendEvent(受伤事件);
            }
        }

    }
}
