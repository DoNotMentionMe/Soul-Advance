
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskCategory("Custom")]
    [TaskDescription("播放粒子效果")]
    public class Play_ParticleSystem : Action
    {
        [SerializeField] ParticleSystem particleSystem;

        public override TaskStatus OnUpdate()
        {
            particleSystem.Play();
            return TaskStatus.Success;
        }
    }
}
