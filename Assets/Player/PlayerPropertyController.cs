using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    /// <summary>
    /// 这个组件对外提供修改真正玩家属性的API
    /// </summary>
    public class PlayerPropertyController : CharacterProperty, IBeAttacked
    {
        [NaughtyAttributes.Expandable]
        [SerializeField]
        PlayerProperty property;

        public override int Attack { get => property.Attack; protected set => Attack = value; }

        protected override void OnEnable()
        {

        }

        public override void BeAttacked(int damage)
        {
            property.BeAttacked(damage);
            Debug.Log($"玩家受伤，当前血量{property.HP}");
        }
    }
}
