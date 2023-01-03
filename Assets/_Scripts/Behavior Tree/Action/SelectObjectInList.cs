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
        [SerializeField] bool 结果不跟当前保存结果相同;
        [SerializeField] SharedTransformList transformList;
        [SerializeField] SharedTransform 保存结果;

        private List<Transform> currentList;

        public override void OnStart()
        {
            currentList = transformList.Value;
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
            if (结果不跟当前保存结果相同 && currentList[index] == 保存结果.Value)
            {
                return GetObject();
            }
            else
                return currentList[index];
        }
    }
}
