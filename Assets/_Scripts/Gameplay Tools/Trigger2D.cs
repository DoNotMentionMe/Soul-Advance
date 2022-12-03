using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger2D : MonoBehaviour
{
    public bool IsTriggered
    {
        get
        {
            if (TriggeredWithTrigger)
                return isTriggeredWithLayer;
            else
                return Physics2D.LinecastNonAlloc(mTransform.position, mTransform.position + Vector3.right * RaycastLength * parentTransform.localScale.x, colls, layers) != 0;
        }
    }
    public float Length => mCol.size.x;
    public Vector2 Pos => mTransform.position;

    [Header("Debug")]
    [SerializeField] bool isTriggeredWithLayer;
    [Header("选择触发类型")]
    [SerializeField] bool TriggeredWithTrigger;
    [SerializeField] bool TirggeredWithRaycast;
    [Header("属性设置")]
    [SerializeField] LayerMask layers;
    [SerializeField] float RaycastLength;
    [Header("组件")]
    [SerializeField] Transform mTransform;
    [SerializeField] Transform parentTransform;
    // public UnityEvent OnTriggerEnter = new UnityEvent();
    // public UnityEvent OnTriggerExit = new UnityEvent();
    // public UnityEvent<Collider2D> OnTriggerEnterWithCollider = new UnityEvent<Collider2D>();
    // public UnityEvent<Collider2D> OnTriggerExitWithCollider = new UnityEvent<Collider2D>();

    private BoxCollider2D mCol;

    private RaycastHit2D[] colls;
    private HashSet<Collider2D> mCollider2Ds = new HashSet<Collider2D>();

    private void OnDrawGizmosSelected()
    {
        if (!TirggeredWithRaycast) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(mTransform.position, mTransform.position + Vector3.right * RaycastLength);
    }

    private void Awake()
    {
        mCol = GetComponent<BoxCollider2D>();
    }

    private void OnDestroy()
    {
        mCol = null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!LayerMaskUtility.Contains(layers, col.gameObject.layer)) return;

        mCollider2Ds.Add(col);

        if (!isTriggeredWithLayer && mCollider2Ds.Count > 0)
        {
            isTriggeredWithLayer = true;
            // OnTriggerEnter?.Invoke();
            // OnTriggerEnterWithCollider?.Invoke(col);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!LayerMaskUtility.Contains(layers, col.gameObject.layer)) return;

        mCollider2Ds.Remove(col);

        if (isTriggeredWithLayer && mCollider2Ds.Count == 0)
        {
            isTriggeredWithLayer = false;
            // OnTriggerExit?.Invoke();
            // OnTriggerExitWithCollider?.Invoke(col);
        }
    }
}
