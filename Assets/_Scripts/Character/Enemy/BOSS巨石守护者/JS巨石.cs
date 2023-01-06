using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace Adv
{
    public class JS巨石 : MonoBehaviour
    {
        [SerializeField, Range(1, 3)] int 巨石所属阶数 = 1;
        [SerializeField, Range(1, 3)] int 巨石所属组数 = 1;
        [SerializeField] IntBoolEventChannel 阶数对应激活事件频道;
        [SerializeField] IntTransformListEventChannel 阶数对应静止点事件频道;
        [Foldout("组件")][SerializeField] SpriteRenderer spriteRenderer;
        [Foldout("组件")][SerializeField] BoxCollider2D boxCollider2D;
        [SerializeField] Transform 静止点;
        [SerializeField] List<Vector3> 静止点位置;
        [SerializeField] float FadeInOrOutDuration = 1f;

        private Coroutine FadeInOrOutCoroutine;

        public void StartFadeInOrOut(bool enabled)
        {
            //TODO开始缓入或缓出协程
            if (FadeInOrOutCoroutine != null)
                StopCoroutine(FadeInOrOutCoroutine);
            FadeInOrOutCoroutine = StartCoroutine(FadeInOrOut(enabled));
            //Test
            // spriteRenderer.enabled = enabled;
            // boxCollider2D.enabled = enabled;
        }

        IEnumerator FadeInOrOut(bool enabled)
        {
            if (enabled)
                spriteRenderer.enabled = enabled;
            float a = spriteRenderer.color.a;
            while (enabled && a < 1f)
            {
                a += 3f * Time.deltaTime;
                var curColor = spriteRenderer.color;
                curColor.a = a;
                spriteRenderer.color = curColor;

                if (!boxCollider2D.enabled && a >= 0.75f)
                    boxCollider2D.enabled = enabled;
                yield return null;
            }

            while (!enabled && a > 0f)
            {
                a -= 0.8f * Time.deltaTime;
                var curColor = spriteRenderer.color;
                curColor.a = a;
                spriteRenderer.color = curColor;
                if (boxCollider2D.enabled && a <= 0.1f)
                    boxCollider2D.enabled = enabled;
                yield return null;
            }

            if (!enabled)
                spriteRenderer.enabled = enabled;

            FadeInOrOutCoroutine = null;
        }

        private void Listen静止点获取频道(int ZS组数, List<Transform> list)
        {
            if (ZS组数 == 巨石所属组数)
            {
                //把静止点加到list中
                静止点.position = 静止点位置[Random.Range(0, 2)];
                list.Add(静止点);
            }
        }

        private void Listen激活频道(int ZS组数, bool enabled)
        {
            if (ZS组数 == 巨石所属组数)
            {
                StartFadeInOrOut(enabled);
            }
        }

        private void Awake()
        {
            阶数对应激活事件频道.AddListener(Listen激活频道);
            阶数对应静止点事件频道.AddListener(Listen静止点获取频道);
        }

        private void OnDestroy()
        {
            阶数对应激活事件频道.RemoveListenner(Listen激活频道);
            阶数对应静止点事件频道.AddListener(Listen静止点获取频道);
        }

        [Button]
        public void 配置组件()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            if (巨石所属阶数 == 1)
            {
                阶数对应激活事件频道 = Resources.Load<IntBoolEventChannel>("一阶巨石组激活频道");
                阶数对应静止点事件频道 = Resources.Load<IntTransformListEventChannel>("一阶静止点获取频道");
            }
            else if (巨石所属阶数 == 2)
            {
                阶数对应激活事件频道 = Resources.Load<IntBoolEventChannel>("二阶巨石组激活频道");
                阶数对应静止点事件频道 = Resources.Load<IntTransformListEventChannel>("二阶静止点获取频道");
            }
            else if (巨石所属阶数 == 3)
            {
                阶数对应激活事件频道 = Resources.Load<IntBoolEventChannel>("三阶巨石组激活频道");
                阶数对应静止点事件频道 = Resources.Load<IntTransformListEventChannel>("三阶静止点获取频道");
            }
            if (静止点 == null)
            {
                静止点 = new GameObject("静止点").transform;
                静止点.parent = transform;
            }
            静止点.localPosition = Vector3.zero;
            静止点位置.Clear();
            静止点位置.Add(new Vector3(transform.position.x - 2.3f, transform.position.y + 4));
            静止点位置.Add(new Vector3(transform.position.x + 2.3f, transform.position.y + 4));
        }


    }
}
