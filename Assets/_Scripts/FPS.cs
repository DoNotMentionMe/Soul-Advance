using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class FPS : MonoBehaviour
    {
        [SerializeField] int targetFrameRate = 60;
        private void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}
