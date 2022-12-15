using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/EventChannels/EnemyGenerateEventChannel"), fileName = ("EnemyGenerateEventChannel_"))]
    public class EnemyGenerateEventChannel : OneParameterEventChannel<EnemyGenerate>
    {

    }
}
