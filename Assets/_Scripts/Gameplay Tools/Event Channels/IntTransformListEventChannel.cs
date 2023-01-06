using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/EventChannels/IntTransformListEventChannels"), fileName = ("IntTransformListEventChannel_"))]
    public class IntTransformListEventChannel : TwoParameterEventChannel<int, List<Transform>>
    {


    }
}
