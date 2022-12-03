using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = "Data/EventChannels/VoidEventChannels", fileName = "VoidEventChannel_")]
    public class VoidEventChannel : ScriptableObject
    {
        [SerializeField, TextArea(2, 5)] string comment;
        event System.Action Delegate;

        public void Broadcast()
        {
            Delegate?.Invoke();
        }

        public void AddListener(System.Action action)
        {
            Delegate += action;
        }

        public void RemoveListenner(System.Action action)
        {
            Delegate -= action;
        }
    }
}