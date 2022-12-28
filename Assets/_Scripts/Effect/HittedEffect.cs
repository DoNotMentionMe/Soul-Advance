using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    public class HittedEffect : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] float StartWaitTime = 0.07f;//和玩家顿帧时间相同
        [SerializeField] float LifeTime = 1f;
        [ShowNonSerializedField] private Transform defaultParent;
        private WaitForSecondsRealtime waitForStartWaitTime;
        private WaitForSecondsRealtime waitForLifeTime;
        private const string Idle = "Idle";
        private const string 命中特效 = "命中特效";

        private void Awake()
        {
            defaultParent = transform.parent;
            waitForStartWaitTime = new WaitForSecondsRealtime(StartWaitTime);
            waitForLifeTime = new WaitForSecondsRealtime(LifeTime);
        }
        private void OnEnable()
        {
            if (transform.parent != defaultParent)
            {
                transform.parent = defaultParent;
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(nameof(EffectStart));
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator EffectStart()
        {
            anim.Play(Idle);
            yield return waitForStartWaitTime;
            anim.Play(命中特效);
            yield return waitForLifeTime;
            transform.parent = defaultParent;
            gameObject.SetActive(false);
        }
    }
}