using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("随机选择TransformList中的一个")]
    public class SelectTransformInList : Action
    {
        private enum SelectModes
        {
            完全随机,
            结果不和保存结果相同,
            结果不和对比列表相同,
        }
        [SerializeField] SelectModes SelectMode = SelectModes.完全随机;
        [SerializeField] SharedTransformList transformList;
        [SerializeField] SharedTransform 保存结果;
        [SerializeField] SharedTransformList 对比列表;

        private List<Transform> currentList;
        private List<Transform> m对比列表;

        public override void OnStart()
        {
            currentList = transformList.Value;
            if (SelectMode == SelectModes.结果不和对比列表相同)
            {
                m对比列表 = 对比列表.Value;
            }
        }

        public override TaskStatus OnUpdate()
        {
            保存结果.Value = GetObject();
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            currentList = null;
        }

        private Transform GetObject()
        {
            int index = Random.Range(0, transformList.Value.Count);
            if (SelectMode == SelectModes.结果不和保存结果相同 && currentList[index] == 保存结果.Value)
            {
                return GetObject();
            }
            else if (SelectMode == SelectModes.结果不和对比列表相同)
            {
                bool IsRepeat = false;
                foreach (var result in m对比列表)
                {
                    if (result == currentList[index])
                    {
                        IsRepeat = true;
                        break;
                    }
                }
                if (IsRepeat)
                    return GetObject();
                else
                    return currentList[index];
            }
            else
                return currentList[index];
        }
    }
}
