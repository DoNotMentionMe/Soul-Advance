using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using DG.Tweening;

namespace Adv
{
    public class 石头人攻击前残影 : MonoBehaviour
    {
        [SerializeField] MMF_Player feedbacks;
        [SerializeField] float MoveDuration;
        [SerializeField] float MoveXDistance;
        [SerializeField] float MoveYDistance;

        private void OnEnable()
        {
            feedbacks.PlayFeedbacks();
            transform.DOMoveX(transform.position.x - MoveXDistance * transform.localScale.x, MoveDuration);
            transform.DOMoveY(transform.position.y + MoveYDistance, MoveDuration);
        }
    }
}
