using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Adv
{
    public class PlayerFistEffect : MonoBehaviour
    {
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] float StartA;
        [SerializeField] float LifeTime;
        [SerializeField] float MoveSpeed;
        private void OnEnable()
        {
            //往前走。同时组件消失
            var color = spriteRenderer.color;
            var direction = Mathf.Sign(mTransform.localScale.x);
            DOVirtual.DelayedCall(LifeTime, () =>
            {
                color.a = 0;
                spriteRenderer.color = color;
            })
            .OnUpdate(() =>
            {
                color.a -= Time.deltaTime * StartA / LifeTime;
                mTransform.position += Vector3.right * direction * MoveSpeed * Time.deltaTime;
                spriteRenderer.color = color;
            })
            .OnStart(() =>
            {
                color.a = StartA;
                spriteRenderer.color = color;
            })
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        private Transform mTransform;

        private void Awake()
        {
            mTransform = transform;
        }

        private void OnDestroy()
        {
            mTransform = null;
        }

        // IEnumerator Execute()
        // {
        //     yield return null;
        // }
    }
}
