using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class AttackBox : MonoBehaviour
    {
        [SerializeField] bool 独立攻击力 = false;
        [SerializeField] bool 玩家攻击盒 = false;
        [SerializeField][ShowIf("独立攻击力")] int 攻击力;
        [SerializeField][HideIf("独立攻击力")] CharacterProperty property;
        [SerializeField][HideIf("玩家攻击盒")] GameObjectEventChannel On玩家受伤Event;
        [SerializeField][HideIf("玩家攻击盒")] GameObject Attacker;
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
                if (!玩家攻击盒)
                    On玩家受伤Event.Broadcast(Attacker);
                if (BeHittedObj.gameObject.TryGetComponent<IBeAttacked>(out IBeAttacked beAttacked))//如果碰撞体上没有实现这个接口说明是无敌碰撞体
                {
                    if (独立攻击力)
                        beAttacked.BeAttacked(攻击力);
                    else
                        beAttacked.BeAttacked(property.Attack);
                }
            }
        }
    }
}
