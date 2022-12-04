using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/EventChannels/EventChannelManager"), fileName = ("EventChannelManager"))]
    public class EventChannelManager : ScriptableObject
    {
        private List<ScriptableObject> eventChannels = new List<ScriptableObject>();

        private void OnEnable()
        {
            eventChannels = Resources.LoadAll<ScriptableObject>("Event Channels").ToList();
        }

        /// <summary>
        /// 根据name遍历找到事件频道，尽量找到后就储存在本地变量中
        /// </summary>
        public ScriptableObject GetEventChannel(string name)
        {
            for (var i = 0; i < eventChannels.Count; i++)
            {
                if (eventChannels[i].name == name)
                {
                    return eventChannels[i];
                }
            }
            return null;
        }
    }
}
