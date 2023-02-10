using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/PlayerItem/JTJS击退效果减弱"), fileName = ("JTJS击退效果减弱"))]
    public class JTJS击退效果减弱 : SuperItem
    {
        public float JT击退因数 = 0.3f;
        public float JZ僵直时间因数 = 0.3f;
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
            currentPlayer.ctler.HurtBackFactor = JT击退因数;
            currentPlayer.ctler.HurtStateFactor = JZ僵直时间因数;
        }

        public override void EffectRemove()
        {
            currentPlayer.ctler.HurtBackFactor = 1f;
            currentPlayer.ctler.HurtStateFactor = 1f;
            currentPlayer = null;
        }

    }
}
