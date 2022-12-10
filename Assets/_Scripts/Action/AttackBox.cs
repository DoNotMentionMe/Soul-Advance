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
                col.gameObject.GetComponent<IBeAttacked>().BeAttacked();
                AttackHittedEvent?.Invoke();
            }
        }
    }
}
