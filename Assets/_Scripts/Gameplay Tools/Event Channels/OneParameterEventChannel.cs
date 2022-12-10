using UnityEngine;

namespace Adv
{
    public class OneParameterEventChannel<T> : ScriptableObject
    {
        [SerializeField, TextArea(2, 5)] string comment;
        event System.Action<T> Delegate;

        public void Broadcast(T obj)
        {
            Delegate?.Invoke(obj);
        }

        public void AddListener(System.Action<T> action)
        {
            Delegate += action;
        }

        public void RemoveListenner(System.Action<T> action)
        {
            Delegate -= action;
        }
    }
}