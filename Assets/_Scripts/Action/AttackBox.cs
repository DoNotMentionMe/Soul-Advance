using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class AttackBox : MonoBehaviour
    {
        [SerializeField] LayerMask AttackLayer;
        [SerializeField] UnityEvent AttackHittedEvent;
        public void AttackBoxDecide(Collider2D col)
        {
            if (LayerMaskUtility.Contains(AttackLayer, col.gameObject.layer))
            {
                if (col.gameObject.TryGetComponent<IBeAttacked>(out IBeAttacked beAttacked))//如果碰撞体上没有实现这个接口说明是无敌碰撞体
                {
                    beAttacked.BeAttacked();
                    AttackHittedEvent?.Invoke();
                }
            }
        }
    }
}
