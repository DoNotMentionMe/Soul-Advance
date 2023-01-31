using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class ExecuteOnEnable : MonoBehaviour
    {
        [SerializeField] UnityEvent OnEnableEvent = new UnityEvent();

        private void OnEnable()
        {
            OnEnableEvent?.Invoke();
        }
    }
}
