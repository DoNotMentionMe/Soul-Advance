using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/PlayerItem/LJJD连击多阶段"), fileName = ("LJJD连击多阶段"))]
    public class LJJD连击多阶段 : SuperItem
    {
        [ReadOnly] public bool Is连击模块 = false;//是否装载连击模块
        [Header("连击数可到达更高阶段")]
        public PlayerProperty playerProperty;
        public FloatEventChannel On玩家连击Event;
        public FloatEventChannel On攻击动画速度变更Event;
        public FloatEventChannel On移动动画速度变更Event;
        [Foldout("连击阶段数据")] public LJSJ连击阶段数据 一阶段数据;
        [Foldout("连击阶段数据")] public LJSJ连击阶段数据 二阶段数据;
        [Foldout("连击阶段数据")] public LJSJ连击阶段数据 三阶段数据;

        private LJJD连击阶段 ljjd连击阶段;

        protected override void OnEnable()
        {
            base.OnEnable();
            //初始化
            ljjd连击阶段 = LJJD连击阶段._1一阶段;
            Is连击模块 = false;
        }

        public void LJ连击阶段模块(float 当前连击数)
        {
            if (ljjd连击阶段 != LJJD连击阶段._3三阶段 && CheckLJ连击数(三阶段数据, null))
            {
                ljjd连击阶段 = LJJD连击阶段._3三阶段;
                ProcessLJSJ连击阶段数据(三阶段数据);
                Debug.Log($"进入第三阶段");
            }
            else if (ljjd连击阶段 != LJJD连击阶段._2二阶段 && CheckLJ连击数(二阶段数据, 三阶段数据))
            {
                ljjd连击阶段 = LJJD连击阶段._2二阶段;
                ProcessLJSJ连击阶段数据(二阶段数据);
                Debug.Log($"进入第二阶段");
            }
            else if (ljjd连击阶段 != LJJD连击阶段._1一阶段 && CheckLJ连击数(一阶段数据, 二阶段数据))
            {
                ljjd连击阶段 = LJJD连击阶段._1一阶段;
                ProcessLJSJ连击阶段数据(一阶段数据);
                Debug.Log($"进入第一阶段");
            }
        }

        private void ProcessLJSJ连击阶段数据(LJSJ连击阶段数据 ljsj)
        {
            playerProperty.BL攻击增长倍率 = ljsj.BL攻击倍率;
            playerProperty.BL能量提升速度倍率 = ljsj.BL能量提升速度倍率;
            playerProperty.BL移速增加倍率 = ljsj.BL移速增加倍率;
            playerProperty.BL攻速倍率 = ljsj.BL攻速倍率;
            On移动动画速度变更Event.Broadcast(playerProperty.BL移速增加倍率);
            On攻击动画速度变更Event.Broadcast(playerProperty.BL攻速倍率);
        }

        private bool CheckLJ连击数(LJSJ连击阶段数据 min, LJSJ连击阶段数据 max)
        {
            if (max == null)
                return playerProperty.DQLJS当前连击数 >= min.JDLJ阶段所需连击数;
            else
                return playerProperty.DQLJS当前连击数 >= min.JDLJ阶段所需连击数 && playerProperty.DQLJS当前连击数 < max.JDLJ阶段所需连击数;
        }

        public override void Effect()
        {
            //装载连击模块
            if (!Is连击模块)
            {
                playerProperty.Remove卸载默认连击阶段模块();
                On玩家连击Event.AddListener(LJ连击阶段模块);
                Is连击模块 = true;
            }
            //初始化连击阶段数据
            ljjd连击阶段 = LJJD连击阶段._1一阶段;
            ProcessLJSJ连击阶段数据(一阶段数据);
            playerProperty.DQLJS当前连击数 = 0;
        }

        public override void EffectRemove()
        {
            //卸载连击模块
            if (Is连击模块)
            {
                On玩家连击Event.RemoveListenner(LJ连击阶段模块);
                playerProperty.Add加载默认连击阶段模块();
                Is连击模块 = false;
            }
            //初始化默认连击阶段数据
            playerProperty.Reset连击阶段();
        }

        [System.Serializable]
        public class LJSJ连击阶段数据
        {
            public int JDLJ阶段所需连击数;
            public float BL攻击倍率;
            public float BL能量提升速度倍率;
            public float BL移速增加倍率;
            public float BL攻速倍率;
        }
    }
}
