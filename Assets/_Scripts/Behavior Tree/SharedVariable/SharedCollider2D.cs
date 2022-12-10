using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [System.Serializable]
    public class SharedCollider2D : SharedVariable<Collider2D>
    {
        public static implicit operator SharedCollider2D(Collider2D value) { return new SharedCollider2D { Value = value }; }
    }
}
