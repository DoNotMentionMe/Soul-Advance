using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/PlayerItem/血魔流主技能"), fileName = ("血魔流主技能"))]
    public class 血魔流主技能 : SuperItem
    {
        public PlayerProperty playerProperty;
        public float ZY增益因子 = 1.5f;
        public float 吸血率 = 5f;
        private PlayerFSM currentPlayer;
        private const string Key = "血魔流主技能";

        protected override void OnEnable()
        {
            base.OnEnable();
            //初始化
            currentPlayer = null;
        }

        private void OnDisable()
        {
            if (currentPlayer != null)
                EffectRemove();
        }

        public override void Effect()
        {
            //开始PlayerPropertyController中持续扣血协程
            currentPlayer = PlayerFSM.Player;
            currentPlayer.propertyController.ContinueDecreaseHP(Key, playerProperty.MaxHP / 100, ExecutePerSec);
            playerProperty.吸血率 += 吸血率;
        }

        public override void EffectRemove()
        {
            currentPlayer.propertyController.StopDecreaseHP(Key);
            playerProperty.吸血率 -= 吸血率;
            currentPlayer = null;
        }

        public void ExecutePerSec()
        {
            var increase = 1 + (playerProperty.MaxHP - playerProperty.HP) / playerProperty.MaxHP * ZY增益因子;
            playerProperty.属性增长修改(SXLX属性类型.GJL攻击力, Key, increase);
            playerProperty.属性增长修改(SXLX属性类型.GS攻速, Key, increase);
        }
    }
}
