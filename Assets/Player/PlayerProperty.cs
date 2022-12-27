using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/Player/PlayerProperty"), fileName = ("PlayerProperty"))]
    public class PlayerProperty : ScriptableObject
    {
        [Header("角色数据")]
        public int Attack;
        [SerializeField] int InitialHP;//初始血量

        [SerializeField] int ExtraHP;//额外血量
        [ReadOnly] public int HP;//当前血量

        private void OnEnable()
        {
            HP = InitialHP + ExtraHP;
        }

        public void BeAttacked(int damage)
        {
            ReduceHP(damage);
        }

        protected virtual void ReduceHP(int minus)
        {
            HP -= minus;
        }

        [Button]
        public void FullHP()
        {
            HP = InitialHP + ExtraHP;
        }
    }
}
