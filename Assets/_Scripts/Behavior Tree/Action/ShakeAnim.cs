using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using DG.Tweening;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("横纵方向震动动画")]
    public class ShakeAnim : Action
    {
        [SerializeField] SharedApPortrait mApPortrait;
        [SerializeField] SharedFloat ShakeTime;
        [SerializeField] SharedVector2 ShakeStrength;

        private Transform animTransform;

        public override void OnAwake()
        {
            animTransform = mApPortrait.Value.transform;
        }

        public override TaskStatus OnUpdate()
        {
            animTransform.DOShakePosition(ShakeTime.Value, ShakeStrength.Value);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            animTransform = null;
        }
    }
}
