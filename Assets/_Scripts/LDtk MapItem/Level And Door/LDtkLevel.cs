using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class LDtkLevel : MonoBehaviour
    {
        public List<Door_LDtk> DoorList = new List<Door_LDtk>();

        private void Awake()
        {
            DoorList = GetComponentsInChildren<Door_LDtk>().ToList();
        }

        public Door_LDtk GetDoorWithType(LDtkDoorType doortype)
        {
            for (var i = 0; i < DoorList.Count; i++)
            {
                if (DoorList[i].DoorType == doortype)
                {
                    return DoorList[i];
                }
            }
            return null;
        }
    }
}
