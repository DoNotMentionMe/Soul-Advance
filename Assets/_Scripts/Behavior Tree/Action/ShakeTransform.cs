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
    public class ShakeTransform : Action
    {
        [SerializeField] SharedTransform TargetTransform;
        [SerializeField] SharedFloat DelayTime;
        [SerializeField] SharedFloat Duration;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("震动强度")]
        [SerializeField] SharedVector3 Strength;//震动强度
        [BehaviorDesigner.Runtime.Tasks.Tooltip("震动次数")]
        [SerializeField] SharedInt Vibrato = 10;//震动次数
        [BehaviorDesigner.Runtime.Tasks.Tooltip("震动随机数")]
        [SerializeField, Range(0, 180)] SharedFloat Randomness = 90;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("平滑所有值为整数")]
        [SerializeField] SharedBool Snapping = false;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("持续时间内逐渐淡出")]
        [SerializeField] SharedBool FadeOut = true;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("随机模式")]
        [SerializeField] ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full;


        public override TaskStatus OnUpdate()
        {
            DOVirtual.DelayedCall(DelayTime.Value, () =>
            {
                TargetTransform.Value.DOShakePosition(Duration.Value, Strength.Value, Vibrato.Value, Randomness.Value, Snapping.Value, FadeOut.Value, randomnessMode);
            });
            return TaskStatus.Success;
        }
    }
}
