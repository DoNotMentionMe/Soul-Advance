using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;

namespace Adv
{
    public class Action_PlayAnim : MonoBehaviour
    {
        private apPortrait mApPortrait;

        private void Awake()
        {
            mApPortrait = GetComponent<apPortrait>();
        }

        public void PlayAnim(string AnimName)
        {
            mApPortrait.CrossFade(AnimName, 0f);
        }
    }
}
