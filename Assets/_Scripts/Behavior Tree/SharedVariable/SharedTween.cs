
using BehaviorDesigner.Runtime;
using DG.Tweening;

namespace Adv
{
    public class SharedTween : SharedVariable<Tween>
    {
        public static implicit operator SharedTween(Tween value) { return new SharedTween { Value = value }; }
    }
}
