using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using BehaviorDesigner.Runtime;
using DG.Tweening;

namespace Adv
{
    /// <summary>
    /// 控制巨石组变化
    /// </summary>
    public class JSSHZ巨石守护者 : MonoBehaviour
    {
        public List<Transform> 当前巨石组 { get; set; }//和行为树的当前巨石组直接链接

        [SerializeField] IntBoolEventChannel 一阶巨石组频道;
        [SerializeField] IntBoolEventChannel 二阶巨石组频道;
        [SerializeField] IntBoolEventChannel 三阶巨石组频道;
        [SerializeField] BOSSProperty bOSSProperty;
        [SerializeField] BehaviorTree mTree;

        #region 巨石组
        [Foldout("各巨石组父级")][SerializeField] Transform One一阶巨石组1父级;
        [Foldout("各巨石组父级")][SerializeField] Transform Two二阶巨石组1父级;
        [Foldout("各巨石组父级")][SerializeField] Transform Two二阶巨石组2父级;
        [Foldout("各巨石组父级")][SerializeField] Transform Three三阶巨石组1父级;
        [Foldout("各巨石组父级")][SerializeField] Transform Three三阶巨石组2父级;
        [Foldout("各巨石组父级")][SerializeField] Transform Three三阶巨石组3父级;
        [Foldout("各巨石组")][SerializeField] List<Transform> One一阶巨石组1;
        [Foldout("各巨石组")][SerializeField] List<Transform> Two二阶巨石组1;
        [Foldout("各巨石组")][SerializeField] List<Transform> Two二阶巨石组2;
        [Foldout("各巨石组")][SerializeField] List<Transform> Three三阶巨石组1;
        [Foldout("各巨石组")][SerializeField] List<Transform> Three三阶巨石组2;
        [Foldout("各巨石组")][SerializeField] List<Transform> Three三阶巨石组3;
        #endregion

        /// <summary>
        /// 用在编辑器中一键配置所有巨石组
        /// </summary>
        [Button]
        public void 装配所有巨石组()
        {
            装配巨石组(One一阶巨石组1, One一阶巨石组1父级);
            装配巨石组(Two二阶巨石组1, Two二阶巨石组1父级);
            装配巨石组(Two二阶巨石组2, Two二阶巨石组2父级);
            装配巨石组(Three三阶巨石组1, Three三阶巨石组1父级);
            装配巨石组(Three三阶巨石组2, Three三阶巨石组2父级);
            装配巨石组(Three三阶巨石组3, Three三阶巨石组3父级);
        }

        private int SZJD当前巨石组所在阶段;
        private int SZZS当前巨石组所在组数;

        /// <summary>
        /// 当阶段改变或生成新巨石组时调用
        /// </summary>
        /// <returns></returns>
        public void Enable随机激活当前阶段巨石组()
        {
            //关闭当前巨石组
            Enable激活指定阶段指定巨石组(SZJD当前巨石组所在阶段, SZZS当前巨石组所在组数, false);

            //随机一个巨石组，并赋值给当前巨石组
            var index = GetRandomIndex();//一阶段1种巨石组，二阶段2种巨石组，三阶段3中巨石组

            //开启当前巨石组
            Enable激活指定阶段指定巨石组(bOSSProperty.当前阶段, index, true);
        }

        public void Enable激活指定阶段指定巨石组(int ZDJD指定阶段, int index, bool enabled)
        {
            switch (ZDJD指定阶段)
            {
                case 1:
                    一阶巨石组频道.Broadcast(index, enabled);
                    break;
                case 2:
                    二阶巨石组频道.Broadcast(index, enabled);
                    break;
                case 3:
                    三阶巨石组频道.Broadcast(index, enabled);
                    break;
                default:
                    break;
            }
            if (enabled)
            {
                SZJD当前巨石组所在阶段 = ZDJD指定阶段;
                SZZS当前巨石组所在组数 = index;
                设置当前巨石组(SZJD当前巨石组所在阶段, SZZS当前巨石组所在组数);
                Debug.Log($"激活{SZJD当前巨石组所在阶段}阶{SZZS当前巨石组所在组数}组");
            }
        }


        private void OnEnable()
        {
            //当前巨石组 = One一阶巨石组1;
            SZJD当前巨石组所在阶段 = 1;
            SZZS当前巨石组所在组数 = 1;
            //bOSSProperty.On阶段改变.AddListener(Listen阶段改变);
            Enable激活指定阶段指定巨石组(SZJD当前巨石组所在阶段, SZZS当前巨石组所在组数, true);
            DOVirtual.DelayedCall(0.1f, () =>
            {
                mTree.EnableBehavior();
            });

        }

        private void OnDisable()
        {
            //bOSSProperty.On阶段改变.RemoveListener(Listen阶段改变);
        }

        // private void Listen阶段改变(int 新阶段)
        // {
        //     //阶段改变时需要过度阶段
        //     Enable随机激活当前阶段巨石组();
        // }

        /// <summary>
        /// 获取一个不跟当前组数相同的组数，除非当前阶段和当前巨石所在阶段不同
        /// </summary>
        /// <returns></returns>
        private int GetRandomIndex()
        {
            if (bOSSProperty.当前阶段 != SZJD当前巨石组所在阶段 || bOSSProperty.当前阶段 == 1)
                return Random.Range(1, bOSSProperty.当前阶段 + 1);
            else
            {
                var index = Random.Range(1, bOSSProperty.当前阶段 + 1);
                if (index == SZZS当前巨石组所在组数)
                    return GetRandomIndex();
                else
                    return index;
            }
        }

        /// <summary>
        /// 根据输入阶段数和组数重新设置“当前巨石组”
        /// </summary>
        private void 设置当前巨石组(int DQJD当前阶段, int DQZS当前组数)
        {
            if (DQJD当前阶段 == 1)
                当前巨石组 = One一阶巨石组1;
            else if (DQJD当前阶段 == 2)
            {
                if (DQZS当前组数 == 1)
                    当前巨石组 = Two二阶巨石组1;
                else if (DQZS当前组数 == 2)
                    当前巨石组 = Two二阶巨石组2;
            }
            else if (DQJD当前阶段 == 3)
            {
                if (DQZS当前组数 == 1)
                    当前巨石组 = Three三阶巨石组1;
                else if (DQZS当前组数 == 2)
                    当前巨石组 = Three三阶巨石组2;
                else if (DQZS当前组数 == 3)
                    当前巨石组 = Three三阶巨石组3;
            }
        }

        private void 装配巨石组(List<Transform> N阶巨石组, Transform N阶巨石组父级)
        {
            N阶巨石组.Clear();
            for (var i = 0; i < N阶巨石组父级.childCount; i++)
            {
                N阶巨石组.Add(N阶巨石组父级.GetChild(i));
            }
        }
    }
}
