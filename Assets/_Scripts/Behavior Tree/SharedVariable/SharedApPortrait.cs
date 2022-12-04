using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;
using BehaviorDesigner.Runtime;

namespace Adv
{
    [System.Serializable]
    public class SharedApPortrait : SharedVariable<apPortrait>
    {
        public static implicit operator SharedApPortrait(apPortrait value) { return new SharedApPortrait { Value = value }; }
    }
}
