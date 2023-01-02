using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class AnimatorPlay : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] string AnimName;

        public void Play()
        {
            animator.Play(AnimName);
        }
    }
}
