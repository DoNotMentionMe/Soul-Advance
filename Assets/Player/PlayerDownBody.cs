using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerDownBody : MonoBehaviour
    {

        [SerializeField] CapsuleCollider2D mColl;

        public void SetCollEnable(bool enable) => mColl.enabled = enable;
    }
}
