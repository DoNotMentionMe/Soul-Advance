using UnityEngine;
using DG.Tweening;

namespace Adv
{
    public class PlayerAttackShadow : MonoBehaviour
    {
        [SerializeField] Vector2 ReleaseOffset;
        [SerializeField] float LifeTime;
        [SerializeField] SpriteRenderer mRenderer;
        [SerializeField] Color ColorFrom;
        [SerializeField] Color ColorTo;

        private void OnEnable()
        {
            //调整残影位置和玩家重合
            var releaseOffset = ReleaseOffset;
            if (mTransform.localScale.x < 0)
                releaseOffset.x *= -1;
            mTransform.position += (Vector3)releaseOffset;
            //改变透明度
            mRenderer.DORestart();
        }

        private Transform mTransform;

        private void Awake()
        {
            mTransform = transform;
            mRenderer
                .DOColor(ColorTo, LifeTime)
                .OnPlay(() =>
                {
                    mRenderer.color = ColorFrom;
                })
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                })
                .SetAutoKill(false);
        }

        private void OnDestroy()
        {
            mTransform = null;
            mRenderer.DOKill();
        }
    }
}
