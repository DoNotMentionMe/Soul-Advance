using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public abstract class Door_LDtk : MonoBehaviour
    {
        public abstract LDtkDoorType DoorType { get; set; }

        public Vector2 levelPositionOffset;
        protected bool HasBeUsed = false;

        protected virtual void Awake()
        {
            levelPositionOffset = transform.parent.position - transform.position;
        }
    }

    public enum LDtkDoorType
    {
        Left,
        Right,
        Up,
        Down,
    }
}
