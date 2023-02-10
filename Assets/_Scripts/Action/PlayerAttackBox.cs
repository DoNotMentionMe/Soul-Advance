using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class PlayerAttackBox : MonoBehaviour
    {
        [SerializeField] bool 独立攻击力 = false;
        [SerializeField][ShowIf("独立攻击力")] int 攻击力;
        [SerializeField][HideIf("独立攻击力")] PlayerProperty property;
        [SerializeField][HideIf("独立攻击力")] PlayerPropertyController propertyController;
        [SerializeField] LayerMask AttackLayer;
        [SerializeField] UnityEvent AttackHittedEvent;
        [SerializeField] UnityEvent<Collider2D> AttackHittedEventWithColl;

        private bool Can获取能量 = true;

        public void AttackBoxDecide(Collider2D BeHittedObj)
        {
            if (LayerMaskUtility.Contains(AttackLayer, BeHittedObj.gameObject.layer))
            {
                //先播放攻击方的攻击命中效果，再播放受伤方的受伤效果
                AttackHittedEvent?.Invoke();
                AttackHittedEventWithColl?.Invoke(BeHittedObj);
                if (BeHittedObj.gameObject.TryGetComponent<IBeAttacked>(out IBeAttacked beAttacked))//如果碰撞体上没有实现这个接口说明是无敌碰撞体
                {
                    if (独立攻击力)
                        beAttacked.BeAttacked(攻击力);
                    else
                        beAttacked.BeAttacked(propertyController.Attack);

                    if (Can获取能量)
                    {
                        Can获取能量 = false;
                        property.NLTS能量提升();
                        property.DQLJS当前连击数 += 1;
                        propertyController.StartDQS清空连击数();
                    }
                }
            }
        }

        /// <summary>
        /// 攻击碰撞体Trigger的OnSetCollFalse拖拽调用
        /// </summary>
        public void 获取能量开关() => Can获取能量 = true;
    }
}
