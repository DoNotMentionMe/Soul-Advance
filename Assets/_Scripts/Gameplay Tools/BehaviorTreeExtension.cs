
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    /// <summary>
    /// 功能：1、延迟设置行为树的Bool类型变量
    /// </summary>
    public class BehaviorTreeExtension : MonoBehaviour
    {
        public Dictionary<string, BoolChanged> BoolChangedsDictionary;
        public CardStruct[] boolChangeds;

        private BehaviorTree behaviorTree;

        private void Awake()
        {
            behaviorTree = GetComponent<BehaviorTree>();
            foreach (var BoolChanged in boolChangeds)
            {
                BoolChanged.boolChanged.storeValue = (SharedBool)behaviorTree?.GetVariable(BoolChanged.VariableName);
            }
            CopyToBoolChangeds();
        }

        private void OnEnable()
        {
            // foreach (var BoolChanged in boolChangeds)
            // {
            //     if (BoolChanged.SetValueOnEnable)
            //         StartCoroutine
            // }
        }

        public void SetBool(string VariableName)
        {
            var boolChanged = BoolChangedsDictionary[VariableName];
            if (boolChanged.coroutine != null)
                StopCoroutine(boolChanged.coroutine);
            boolChanged.coroutine = StartCoroutine(SetDelay(boolChanged));
        }

        // IEnumerator SetDelay(float delay, BoolChanged boolChanged)
        // {
        //     yield return new WaitForSeconds(delay);
        //     boolChanged.storeValue.Value = boolChanged.SetValue;
        // }

        IEnumerator SetDelay(BoolChanged boolChanged)
        {
            yield return new WaitForSeconds(boolChanged.SetDelay);
            boolChanged.storeValue.Value = boolChanged.SetValue;
            boolChanged.coroutine = null;
        }

        [System.Serializable]
        public class BoolChanged
        {
            public bool SetValue;
            public float SetDelay;
            [HideInInspector] public SharedBool storeValue;
            public Coroutine coroutine;
        }


        [System.Serializable]
        public struct CardStruct
        {
            // public bool SetValueOnEnable;
            // [ShowIf("SetValueOnEnable")] public float SetDelay;
            public string VariableName;
            public BoolChanged boolChanged;
        }

        private void CopyToBoolChangeds()
        {
            // 字典内容
            BoolChangedsDictionary = new Dictionary<string, BoolChanged>();
            //for (int i = 0; i < boolChangeds.Length; i++)
            foreach (var boolChanged in boolChangeds)
            {
                if (!BoolChangedsDictionary.ContainsKey(boolChanged.VariableName))
                {
                    BoolChangedsDictionary.Add(boolChanged.VariableName, boolChanged.boolChanged);
                }
            }
        }
    }


}
