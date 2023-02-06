using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class WaitTime : MonoBehaviour
    {
        [SerializeField] float waitTime;
        [SerializeField] UnityEvent OnStart;
        [SerializeField] UnityEvent OnEnd;

        private Coroutine coroutine;
        private WaitForSeconds waitForWaitTime;

        private void Awake()
        {
            waitForWaitTime = new WaitForSeconds(waitTime);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            coroutine = null;
        }

        public void Execute()
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(nameof(ExecuteCoroutine));
        }

        public void Execute(float waitTime)
        {
            waitForWaitTime = new WaitForSeconds(waitTime);
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(nameof(ExecuteCoroutine));
        }

        IEnumerator ExecuteCoroutine()
        {
            OnStart?.Invoke();
            yield return waitForWaitTime;
            OnEnd?.Invoke();

            coroutine = null;
        }
    }
}
