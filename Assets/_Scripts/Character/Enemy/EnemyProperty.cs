using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class EnemyProperty : CharacterProperty, IBeAttacked
    {
        public override int Attack { get => attack; protected set => attack = value; }
        [SerializeField] int attack;
        [Header("敌人组件")]
        [SerializeField] protected BehaviorTree mTree;
        [SerializeField] string 受伤事件 = "BeAttacked";
        [SerializeField] string 死亡事件 = "Died";
        [SerializeField] UnityEvent BeHittedEvent;//被命中时调用EnemyBattleEffect.BeHittedEffect
        [SerializeField] UnityEvent DiedEvent;

        public void KillEnemy()
        {
            BeAttacked(HP);
        }


        public override void BeAttacked(int damage)
        {
            base.BeAttacked(damage);
            if (HP <= 0)
            {
                BeHittedEvent.Invoke();
                DiedEvent.Invoke();
                mTree?.SendEvent(死亡事件);
            }
            else
            {
                BeHittedEvent.Invoke();
                mTree?.SendEvent(受伤事件);
            }
        }

        private void Update()
        {
            if (transform.position.y < -200)
            {
                KillEnemy();
            }
        }

    }
}
