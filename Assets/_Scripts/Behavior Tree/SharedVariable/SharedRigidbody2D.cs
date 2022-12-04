using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [System.Serializable]
    public class SharedRigidbody2D : SharedVariable<Rigidbody2D>
    {
        public static implicit operator SharedRigidbody2D(Rigidbody2D value) { return new SharedRigidbody2D { Value = value }; }
    }
}
