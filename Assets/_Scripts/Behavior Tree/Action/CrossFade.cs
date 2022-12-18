using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    public class CrossFade : Action
    {
        [SerializeField] SharedApPortrait anim;
        [SerializeField] string animName;
        [SerializeField] float fadeTime = 0f;

        public override void OnStart()
        {
            anim.Value.CrossFade(animName, fadeTime);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
