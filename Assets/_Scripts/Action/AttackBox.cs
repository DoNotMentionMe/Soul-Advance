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
                if (BeHittedObj.gameObject.TryGetComponent<IBeAttacked>(out IBeAttacked beAttacked))//如果碰撞体上没有实现这个接口说明是无敌碰撞体
                {
                    beAttacked.BeAttacked(property.Attack);
                }
                AttackHittedEvent?.Invoke();
                AttackHittedEventWithColl?.Invoke(BeHittedObj);
            }
        }
    }
}
