using BehaviorDesigner.Runtime;

namespace Adv
{
    [System.Serializable]
    public class SharedTrigger2D : SharedVariable<Trigger2D>
    {
        public static implicit operator SharedTrigger2D(Trigger2D value) { return new SharedTrigger2D { Value = value }; }
    }
}
