using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class StateMachine : MonoBehaviour
    {
        public IState currentState;

        public Dictionary<System.Type, IState> stateTable = new Dictionary<System.Type, IState>();

        #region 状态转换
        public void SwitchOn(System.Type stateKey)
        {
            var newState = stateTable[stateKey];//这里出错，表示对应状态没有初始化
            newState.Enter();
            currentState = newState;
        }

        public virtual void SwitchState(System.Type stateKey)
        {
            currentState.Exit();
            SwitchOn(stateKey);
        }
        #endregion

        #region 生命周期函数
        private void Update()
        {
            currentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            currentState.PhysicUpdate();
        }
        #endregion
    }
}