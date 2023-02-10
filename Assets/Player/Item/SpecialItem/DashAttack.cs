using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Adv
{
    /// <summary>
    /// 翻滚改为前冲
    /// </summary>
    [CreateAssetMenu(menuName = ("Data/PlayerItem/DashAttack"), fileName = ("DashAttack"))]
    public class DashAttack : SuperItem
    {
        [Tooltip("尽量为0.02的倍数")]
        [Header("Roll改DashAttack")]
        public float DashAttackTime;
        public float DashAttackSpeed;

        private PlayerFSM currentPlayer;

        protected override void OnEnable()
        {
            base.OnEnable();
            //初始化
            currentPlayer = null;
        }

        public override void Effect()
        {
            currentPlayer = PlayerFSM.Player;
            currentPlayer.ctler.rollType = RollType.DashAttack;
        }

        public override void EffectRemove()
        {
            currentPlayer.ctler.rollType = RollType.Roll;
            currentPlayer = null;
        }
    }
}
