using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Adv
{
    public class GameController : PersistentSingleton<GameController>
    {
        [SerializeField, Range(0, 1)] float 时停峰值;
        [SerializeField] float 时停渐入时间;
        [SerializeField] float 时停时间;
        [SerializeField] float 时停渐出时间;

        public void StartTimePause()
        {
            StartCoroutine(nameof(TimePuase));
        }

        IEnumerator TimePuase()
        {
            Time.timeScale = 1;
            float timer = 0f;
            while (timer <= 时停渐入时间)
            {
                timer += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(1, 时停峰值, timer / 时停渐入时间);
                yield return null;
            }
            yield return waitFor时停时间;
            timer = 0f;
            while (timer <= 时停渐出时间)
            {
                timer += Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(时停峰值, 1, timer / 时停渐出时间);
                yield return null;
            }
            Time.timeScale = 1;
        }

        private WaitForSecondsRealtime waitFor时停时间;

        protected override void Awake()
        {
            base.Awake();
            waitFor时停时间 = new(时停时间);
        }

        private void OnDestroy()
        {

        }
    }
}
