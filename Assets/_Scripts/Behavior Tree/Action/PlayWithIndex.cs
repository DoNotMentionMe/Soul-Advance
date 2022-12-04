using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using AnyPortrait;

namespace Adv
{
    [TaskCategory("Custom/ApPortrait")]
    [TaskDescription("根据输入的Index播放动画列表中的动画")]
    public class PlayWithIndex : Action
    {
        [SerializeField] SharedApPortrait anim;
        [SerializeField] SharedInt index;

        public override void OnStart()
        {
            anim.Value.Play(anim.Value.AnimationPlayDataList[index.Value]);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
