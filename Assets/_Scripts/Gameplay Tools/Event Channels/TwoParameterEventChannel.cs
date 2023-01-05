using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class TwoParameterEventChannel<T, W> : ScriptableObject
    {
        [SerializeField, TextArea(2, 5)] string comment;
        event System.Action<T, W> Delegate;

        public void Broadcast(T obj1, W obj2)
        {
            Delegate?.Invoke(obj1, obj2);
        }

        public void AddListener(System.Action<T, W> action)
        {
            Delegate += action;
        }

        public void RemoveListenner(System.Action<T, W> action)
        {
            Delegate -= action;
        }
    }
}
