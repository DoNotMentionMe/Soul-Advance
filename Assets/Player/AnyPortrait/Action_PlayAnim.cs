using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnyPortrait;

namespace Adv
{
    public class Action_PlayAnim : MonoBehaviour
    {
        [SerializeField, TextArea(3, 8)] string commet;
        [SerializeField] apPortrait mApPortrait;
        [SerializeField] PlayerAnimManager playerAnimManager;

        public void PlayAnim(string AnimName)
        {
            mApPortrait.CrossFade(AnimName, 0f);
        }
    }
}
