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
            }
        }
        private int 当前连击数;
        [ShowNativeProperty] public int DQNL当前能量 { get; set; }
        [ShowNativeProperty] public float BL攻击增长倍率 { get => 获取增长后属性(攻击增长倍率, 攻击增长倍率增长值); }
        [ShowNativeProperty] public float BL能量提升速度倍率 { get => 获取增长后属性(能量提升速度倍率, 能量提升速度倍率增长值); }
        [ShowNativeProperty] public float BL移速增加倍率 { get => 获取增长后属性(移速增加倍率, 移速增加倍率增长值); }
        [ShowNativeProperty] public float BL攻速倍率 { get => 获取增长后属性(攻速倍率, 攻速倍率增长值); }
        public int MaxHP => InitialHP + ExtraHP;
        [SerializeField] VoidEventChannel On玩家死亡Event;
        [SerializeField] FloatEventChannel On玩家连击Event;
        [SerializeField] FloatEventChannel On攻击动画速度变更Event;
        [SerializeField] FloatEventChannel On移动动画速度变更Event;
        #region 连击加成倍率数据
        [Foldout("倍率数据")][SerializeField] int 进入第二阶段连击数 = 50;
        [Foldout("倍率数据")][Header("攻击力")][SerializeField] float BLONE攻击倍率 = 1f;
        [Foldout("倍率数据")][SerializeField] float BLTWO攻击倍率 = 1.2f;
        [Foldout("倍率数据")][Header("能量提升")][SerializeField] float BLONE能量提升速度倍率 = 1f;
        [Foldout("倍率数据")][SerializeField] float BLTWO能量提升速度倍率 = 2f;
        [Foldout("倍率数据")][Header("移速增加")][SerializeField] float BLONE移速增加倍率 = 1f;
        [Foldout("倍率数据")][SerializeField] float BLTWO移速增加倍率 = 1.5f;
        [Foldout("倍率数据")][Header("攻速增加")][SerializeField] float BLONE攻速倍率 = 1f;
        [Foldout("倍率数据")][SerializeField] float BLTWO攻速倍率 = 1.5f;
        #endregion
        [Header("角色数据")]
        public int Attack;
        [SerializeField] int InitialHP;//初始血量
        [SerializeField] int ExtraHP;//额外血量
        [ReadOnly] public int HP;//当前血量
        public float 吸血率 = 0;
        [SerializeField] int Max能量值 = 100;
        [SerializeField] int NLTS能量提升速度 = 5;
        public float CD清空连击数时间 = 3;
        public int XHNL消耗能量_Roll = 25;
        [Header("角色属性初始数值")]
        [SerializeField] float 攻击增长倍率 = 1;
        [SerializeField] float 能量提升速度倍率 = 1;
        [SerializeField] float 移速增加倍率 = 1;
        [SerializeField] float 攻速倍率 = 1;
        private Dictionary<string, float> 攻击增长倍率增长值 = new Dictionary<string, float>();
        private Dictionary<string, float> 能量提升速度倍率增长值 = new Dictionary<string, float>();
        private Dictionary<string, float> 移速增加倍率增长值 = new Dictionary<string, float>();
        private Dictionary<string, float> 攻速倍率增长值 = new Dictionary<string, float>();
        private const string 基础增长值索引 = "基础增长值索引";


        private bool Is默认连击模块 = false;//是否装载了默认连击模块
        private LJJD连击阶段 ljjd连击阶段 = LJJD连击阶段._1一阶段;

        private void OnEnable()
        {
            HP = InitialHP + ExtraHP;
            Is默认连击模块 = false;
        }

        public void ResetProperty()
        {
            HP = InitialHP + ExtraHP;
            Is默认连击模块 = false;
            Reset连击阶段();
        }

        public void 属性增长修改(SXLX属性类型 sxlx, string sy索引, float zzz增长值)
        {
            switch (sxlx)
            {
                case SXLX属性类型.GJL攻击力:
                    if (攻击增长倍率增长值.ContainsKey(sy索引))
                        攻击增长倍率增长值[sy索引] = zzz增长值;
                    else
                        攻击增长倍率增长值.Add(sy索引, zzz增长值);
                    break;
                case SXLX属性类型.GS攻速:
                    if (攻速倍率增长值.ContainsKey(sy索引))
                        攻速倍率增长值[sy索引] = zzz增长值;
                    else
                        攻速倍率增长值.Add(sy索引, zzz增长值);
                    break;
                case SXLX属性类型.NLTSSD能量提升速度:
                    if (能量提升速度倍率增长值.ContainsKey(sy索引))
                        能量提升速度倍率增长值[sy索引] = zzz增长值;
                    else
                        能量提升速度倍率增长值.Add(sy索引, zzz增长值);
                    break;
                case SXLX属性类型.YS移速:
                    if (移速增加倍率增长值.ContainsKey(sy索引))
                        移速增加倍率增长值[sy索引] = zzz增长值;
                    else
                        移速增加倍率增长值.Add(sy索引, zzz增长值);
                    break;
            }

        }

        public float 获取增长后属性(float cssx初始属性, Dictionary<string, float> zzz增长值)
        {
            var newValue = cssx初始属性;
            foreach (var zzz in zzz增长值)
            {
                newValue *= zzz.Value;
            }
            //Debug.Log($"{newValue}");
            return newValue;
        }

        public void Reset连击阶段()
        {
            ljjd连击阶段 = LJJD连击阶段._1一阶段;
            // BL攻击增长倍率 = BLONE攻击倍率;
            // BL能量提升速度倍率 = BLONE能量提升速度倍率;
            // BL移速增加倍率 = BLONE移速增加倍率;
            // BL攻速倍率 = BLONE攻速倍率;
            属性增长修改(SXLX属性类型.GJL攻击力, 基础增长值索引, BLONE攻击倍率);
            属性增长修改(SXLX属性类型.NLTSSD能量提升速度, 基础增长值索引, BLONE能量提升速度倍率);
            属性增长修改(SXLX属性类型.YS移速, 基础增长值索引, BLONE移速增加倍率);
            属性增长修改(SXLX属性类型.GS攻速, 基础增长值索引, BLONE攻速倍率);
        }

        public void Add加载默认连击阶段模块()
        {
            if (Is默认连击模块) return;
            On玩家连击Event.AddListener(LJ默认连击阶段模块);
            Is默认连击模块 = true;
        }

        public void Remove卸载默认连击阶段模块()
        {
            if (!Is默认连击模块) return;
            On玩家连击Event.RemoveListenner(LJ默认连击阶段模块);
            Is默认连击模块 = false;
        }

        public void LJ默认连击阶段模块(float 当前连击数)
        {
            if (ljjd连击阶段 != LJJD连击阶段._1一阶段 && 当前连击数 < 进入第二阶段连击数)
            {
                Debug.Log($"默认模块一阶段");
                ljjd连击阶段 = LJJD连击阶段._1一阶段;
                // BL攻击增长倍率 = BLONE攻击倍率;
                // BL能量提升速度倍率 = BLONE能量提升速度倍率;
                // BL移速增加倍率 = BLONE移速增加倍率;
                // BL攻速倍率 = BLONE攻速倍率;
                属性增长修改(SXLX属性类型.GJL攻击力, 基础增长值索引, BLONE攻击倍率);
                属性增长修改(SXLX属性类型.NLTSSD能量提升速度, 基础增长值索引, BLONE能量提升速度倍率);
                属性增长修改(SXLX属性类型.YS移速, 基础增长值索引, BLONE移速增加倍率);
                属性增长修改(SXLX属性类型.GS攻速, 基础增长值索引, BLONE攻速倍率);
                On移动动画速度变更Event.Broadcast(BL移速增加倍率);
                On攻击动画速度变更Event.Broadcast(BL攻速倍率);
            }
            else if (ljjd连击阶段 != LJJD连击阶段._2二阶段 && 当前连击数 >= 进入第二阶段连击数)
            {
                Debug.Log($"默认模块二阶段");
                ljjd连击阶段 = LJJD连击阶段._2二阶段;
                // BL攻击增长倍率 = BLTWO攻击倍率;
                // BL能量提升速度倍率 = BLTWO能量提升速度倍率;
                // BL移速增加倍率 = BLTWO移速增加倍率;
                // BL攻速倍率 = BLTWO攻速倍率;
                属性增长修改(SXLX属性类型.GJL攻击力, 基础增长值索引, BLTWO攻击倍率);
                属性增长修改(SXLX属性类型.NLTSSD能量提升速度, 基础增长值索引, BLTWO能量提升速度倍率);
                属性增长修改(SXLX属性类型.YS移速, 基础增长值索引, BLTWO移速增加倍率);
                属性增长修改(SXLX属性类型.GS攻速, 基础增长值索引, BLTWO攻速倍率);
                On移动动画速度变更Event.Broadcast(BL移速增加倍率);
                On攻击动画速度变更Event.Broadcast(BL攻速倍率);
            }
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
            if (HP <= 0)//死掉
            {
                On玩家死亡Event.Broadcast();
                return true;
            }
            else
                return false;
        }

        public void IncreaseHP(int HPUp)
        {
            HP += HPUp;
            if (HP > MaxHP)
                HP = MaxHP;
        }

        [Button]
        public void FullHP()
        {
            HP = InitialHP + ExtraHP;
            DQNL当前能量 = Max能量值;
        }
    }

    public enum LJJD连击阶段
    {
        _1一阶段,
        _2二阶段,
        _3三阶段,
        _4四阶段,
        _5五阶段,
    }

    public enum SXLX属性类型
    {
        GJL攻击力,
        NLTSSD能量提升速度,
        YS移速,
        GS攻速,
    }
}
