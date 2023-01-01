using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    public class SharedPlayerFSM : SharedVariable<PlayerFSM>
    {
        public static implicit operator SharedPlayerFSM(PlayerFSM value) { return new SharedPlayerFSM { Value = value }; }
    }
}
