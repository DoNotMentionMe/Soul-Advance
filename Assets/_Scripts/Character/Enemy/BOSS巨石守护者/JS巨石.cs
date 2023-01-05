using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Adv
{
    public class JS巨石 : MonoBehaviour
    {
        [SerializeField, Range(1, 3)] int 巨石所属阶数 = 1;
        [SerializeField, Range(1, 3)] int 巨石所属组数 = 1;
        [SerializeField] IntBoolEventChannel 阶数对应事件频道;
        [Foldout("组件")][SerializeField] SpriteRenderer spriteRenderer;
        [Foldout("组件")][SerializeField] BoxCollider2D boxCollider2D;

        private void EventChannelListener(int ZS组数, bool enabled)
        {
            if (ZS组数 == 巨石所属组数)
            {
                //TODO执行开启关闭协程，如果执行时协程正在进行（刚切换巨石组时阶段改变了），直接停止开启新的
                StartFadeInOrOut(enabled);
            }
        }

        public void StartFadeInOrOut(bool enabled)
        {
            //TODO开始缓入或缓出协程
            //Test
            spriteRenderer.enabled = enabled;
            boxCollider2D.enabled = enabled;
        }

        private void Awake()
        {
            阶数对应事件频道.AddListener(EventChannelListener);
        }

        private void OnDestroy()
        {
            阶数对应事件频道.RemoveListenner(EventChannelListener);
        }

        [Button]
        public void 配置组件()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            if (巨石所属阶数 == 1)
                阶数对应事件频道 = Resources.Load<IntBoolEventChannel>("一阶巨石组频道");
            else if (巨石所属阶数 == 2)
                阶数对应事件频道 = Resources.Load<IntBoolEventChannel>("二阶巨石组频道");
            else if (巨石所属阶数 == 3)
                阶数对应事件频道 = Resources.Load<IntBoolEventChannel>("三阶巨石组频道");
        }


    }
}
