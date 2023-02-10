using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/PlayerItem/XP下劈"), fileName = ("XP下劈"))]
    public class XP下劈 : SuperItem
    {
        PlayerFSM currentPlayer;

        protected override void OnEnable()
        {
            base.OnEnable();
            //初始化
            currentPlayer = null;
        }

        public override void Effect()
        {
            currentPlayer = PlayerFSM.Player;
            currentPlayer.ctler.CanDownAttack = true;
        }

        public override void EffectRemove()
        {
            currentPlayer.ctler.CanDownAttack = false;
            currentPlayer = null;
        }
    }
}
