using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = "Data/EventChannels/GameObjectEventChannels", fileName = "GameObjectEventChannel_")]
    public class GameObjectEventChannel : OneParameterEventChannel<GameObject>
    {

    }
}