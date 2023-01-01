using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class AttackBox : MonoBehaviour
    {
        [SerializeField] CharacterProperty property;
        [SerializeField] LayerMask AttackLayer;
        [SerializeField] UnityEvent AttackHittedEvent;
        [SerializeField] UnityEvent<Collider2D> AttackHittedEventWithColl;
        public void AttackBoxDecide(Collider2D BeHittedObj)
        {
            if (LayerMaskUtility.Contains(AttackLayer, BeHittedObj.gameObject.layer))
            {
                //先播放攻击方的攻击命中效果，再播放受伤方的受伤效果
                AttackHittedEvent?.Invoke();
                AttackHittedEventWithColl?.Invoke(BeHittedObj);
                if (BeHittedObj.gameObject.TryGetComponent<IBeAttacked>(out IBeAttacked beAttacked))//如果碰撞体上没有实现这个接口说明是无敌碰撞体
                {
                    beAttacked.BeAttacked(property.Attack);
                }
            }
        }
    }
}
