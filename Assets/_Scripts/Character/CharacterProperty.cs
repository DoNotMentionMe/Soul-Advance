using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public abstract class CharacterProperty : MonoBehaviour
    {
        [Header("角色数据")]
        [SerializeField] protected int MaxHP;

        protected int ExtraHP;//用来记录其他情况增加最大血量的数据
        protected int HP;

        protected virtual void OnEnable()
        {
            HP = MaxHP + ExtraHP;
        }

        public virtual void BeAttacked()
        {
            ReduceHP(1);
        }

        protected virtual void ReduceHP(int minus)
        {
            HP -= minus;
        }
    }
}
