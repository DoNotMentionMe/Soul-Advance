using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/Player/PlayerProperty"), fileName = ("PlayerProperty"))]
    public class PlayerProperty : ScriptableObject
    {
        [ShowNativeProperty]
        public int DQLJS当前连击数
        {
            get
            {
                return 当前连击数;
            }
            set
            {
                当前连击数 = value;
                On玩家连击Event.Broadcast(当前连击数);
                if (当前连击数 < 进入第二阶段连击数)
                {
                    BL攻击增长倍率 = BLONE攻击倍率;
                    BL能量提升速度倍率 = BLONE能量提升速度倍率;
                    BL移速增加倍率 = BLONE移速增加倍率;
                    BL动画倍率 = BLONE动画倍率;
                }
                else if (当前连击数 >= 进入第二阶段连击数)
                {
                    BL攻击增长倍率 = BLTWO攻击倍率;
                    BL能量提升速度倍率 = BLTWO能量提升速度倍率;
                    BL移速增加倍率 = BLTWO移速增加倍率;
                    BL动画倍率 = BLTWO动画倍率;
                }
            }
        }
        private int 当前连击数;
        [ShowNativeProperty] public int DQNL当前能量 { get; set; }
        [ShowNativeProperty] public float BL攻击增长倍率 { get; private set; } = 1;
        [ShowNativeProperty] public float BL能量提升速度倍率 { get; set; } = 1;
        [ShowNativeProperty] public float BL移速增加倍率 { get; set; } = 1;
        [ShowNativeProperty] public float BL动画倍率 { get; set; } = 1;
        [SerializeField] FloatEventChannel On玩家连击Event;
        #region 连击加成倍率数据
        [Foldout("倍率数据")][SerializeField] int 进入第二阶段连击数 = 50;
        [Foldout("倍率数据")][Header("攻击力")][SerializeField] float BLONE攻击倍率 = 1f;
        [Foldout("倍率数据")][SerializeField] float BLTWO攻击倍率 = 1.2f;
        [Foldout("倍率数据")][Header("能量提升")][SerializeField] float BLONE能量提升速度倍率 = 1f;
        [Foldout("倍率数据")][SerializeField] float BLTWO能量提升速度倍率 = 2f;
        [Foldout("倍率数据")][Header("移速增加")][SerializeField] float BLONE移速增加倍率 = 1f;
        [Foldout("倍率数据")][SerializeField] float BLTWO移速增加倍率 = 1.5f;
        [Foldout("倍率数据")][Header("动画速度增加")][SerializeField] float BLONE动画倍率 = 1f;
        [Foldout("倍率数据")][SerializeField] float BLTWO动画倍率 = 1.5f;
        #endregion
        [Header("角色数据")]
        public int Attack;
        [SerializeField] int InitialHP;//初始血量
        [SerializeField] int ExtraHP;//额外血量
        [ReadOnly] public int HP;//当前血量
        [SerializeField] int Max能量值 = 100;
        [SerializeField] int NLTS能量提升速度 = 5;
        public float CD清空连击数时间 = 3;
        public int XHNL消耗能量_Roll = 25;

        private void OnEnable()
        {
            HP = InitialHP + ExtraHP;
        }

        public void NLTS能量提升()
        {
            DQNL当前能量 += (int)(NLTS能量提升速度 * BL能量提升速度倍率);
            if (DQNL当前能量 > Max能量值)
            {
                DQNL当前能量 = Max能量值;
            }
        }

        public void NLJS能量减少()
        {

        }

        public bool NLJS能量减少(int minus)
        {
            if (DQNL当前能量 >= minus)
            {
                DQNL当前能量 -= minus;
                return true;
            }
            else
                return false;
        }

        public bool BeAttacked(int damage)
        {
            HP -= damage;
            if (HP <= 0)
                return true;
            else
                return false;
        }

        [Button]
        public void FullHP()
        {
            HP = InitialHP + ExtraHP;
            DQNL当前能量 = Max能量值;
        }
    }
}
