using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    public class EnemyProperty : CharacterProperty, IBeAttacked
    {
        [Header("敌人组件")]
        [SerializeField] BehaviorTree mTree;



        public override void BeAttacked()
        {
            base.BeAttacked();
            if (HP <= 0)
                mTree.SendEvent("Died");
            else
                mTree.SendEvent("BeAttacked");
        }

    }
}
