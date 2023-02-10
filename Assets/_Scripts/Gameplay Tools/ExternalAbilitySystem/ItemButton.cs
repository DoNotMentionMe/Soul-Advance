using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Adv
{
    public class ItemButton : MonoBehaviour
    {
        public SuperItem Item;

        public void Effect()
        {
            ExternalAbilitySystem.Instance.ZZ装载道具(Item);
        }
    }
}
