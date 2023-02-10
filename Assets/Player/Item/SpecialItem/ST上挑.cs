using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/PlayerItem/ST上挑"), fileName = ("ST上挑"))]
    public class ST上挑 : SuperItem
    {
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
            currentPlayer.ctler.CanUpAttack = true;
        }

        public override void EffectRemove()
        {
            currentPlayer.ctler.CanUpAttack = false;
            currentPlayer = null;
        }
    }
}
