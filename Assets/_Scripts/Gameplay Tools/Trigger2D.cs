using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger2D : MonoBehaviour
{
    public bool IsTriggered => isTriggeredWithLayer;
    public float Length => ((BoxCollider2D)mCol).size.x;
    public Vector2 Pos => mTransform.position;

    [SerializeField] bool isTriggeredWithLayer;
    [SerializeField] LayerMask layers;
    [SerializeField] Transform mTransform;
    // public UnityEvent OnTriggerEnter = new UnityEvent();
    // public UnityEvent OnTriggerExit = new UnityEvent();
    [SerializeField] UnityEvent<Collider2D> OnTriggerEnterWithCollider = new UnityEvent<Collider2D>();
    //public UnityEvent<Collider2D> OnTriggerExitWithCollider = new UnityEvent<Collider2D>();

    private Collider2D mCol;

    private HashSet<Collider2D> mCollider2Ds = new HashSet<Collider2D>();

    private void Awake()
    {
        mCol = GetComponent<Collider2D>();
    }

    private void OnDestroy()
    {
        mCol = null;
        mCollider2Ds.Clear();
        mCollider2Ds = null;
    }

    public void SetCollEnable(bool enable) => mCol.enabled = enable;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!LayerMaskUtility.Contains(layers, col.gameObject.layer)) return;

        if (!mCollider2Ds.Contains(col))
            mCollider2Ds.Add(col);
        OnTriggerEnterWithCollider?.Invoke(col);

        if (!isTriggeredWithLayer && mCollider2Ds.Count > 0)
        {
            isTriggeredWithLayer = true;
            // OnTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!LayerMaskUtility.Contains(layers, col.gameObject.layer)) return;

        if (mCollider2Ds.Contains(col))
            mCollider2Ds.Remove(col);

        if (isTriggeredWithLayer && mCollider2Ds.Count == 0)
        {
            isTriggeredWithLayer = false;
            // OnTriggerExit?.Invoke();
            // OnTriggerExitWithCollider?.Invoke(col);
        }
    }
}
