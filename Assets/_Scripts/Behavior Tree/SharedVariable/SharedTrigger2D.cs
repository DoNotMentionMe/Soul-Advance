using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [System.Serializable]
    public class SharedTrigger2D : SharedVariable<Trigger2D>
    {
        public static implicit operator SharedTrigger2D(Trigger2D value) { return new SharedTrigger2D { Value = value }; }
    }
}
