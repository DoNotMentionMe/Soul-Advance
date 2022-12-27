using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    public abstract class CharacterProperty : MonoBehaviour
    {
        public abstract int Attack { get; protected set; }

        [SerializeField] bool ShowProperty = true;
        [Header("角色数据")]
        [ShowIf("ShowProperty")]
        [SerializeField] protected int MaxHP;

        protected int ExtraHP;//用来记录其他情况增加最大血量的数据
        protected int HP;

        protected virtual void OnEnable()
        {
            HP = MaxHP + ExtraHP;
        }

        public virtual void BeAttacked(int damage)
        {
            ReduceHP(damage);
        }

        protected virtual void ReduceHP(int minus)
        {
            HP -= minus;
        }
    }
}
