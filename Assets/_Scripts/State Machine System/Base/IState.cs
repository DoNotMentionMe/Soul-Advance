using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public interface IState
    {
        void Enter();
        void Exit();
        void LogicUpdate();
        void PhysicUpdate();
    }
}