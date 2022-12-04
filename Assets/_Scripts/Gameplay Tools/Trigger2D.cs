using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger2D : MonoBehaviour
{
    public bool IsTriggered => isTriggeredWithLayer;
    public float Length => mCol.size.x;
    public Vector2 Pos => mTransform.position;
    public Transform LastEnterTransform => lastEnterTransform;

    [Header("Debug")]
    [SerializeField] bool isTriggeredWithLayer;
    [SerializeField] LayerMask layers;
    [SerializeField] Transform mTransform;
    // public UnityEvent OnTriggerEnter = new UnityEvent();
    // public UnityEvent OnTriggerExit = new UnityEvent();
    // public UnityEvent<Collider2D> OnTriggerEnterWithCollider = new UnityEvent<Collider2D>();
    // public UnityEvent<Collider2D> OnTriggerExitWithCollider = new UnityEvent<Collider2D>();

    private BoxCollider2D mCol;
    private Transform lastEnterTransform;

    private HashSet<Collider2D> mCollider2Ds = new HashSet<Collider2D>();

    private void Awake()
    {
        mCol = GetComponent<BoxCollider2D>();
    }

    private void OnDestroy()
    {
        mCol = null;
        mCollider2Ds.Clear();
        mCollider2Ds = null;
        lastEnterTransform = null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!LayerMaskUtility.Contains(layers, col.gameObject.layer)) return;

        mCollider2Ds.Add(col);

        if (!isTriggeredWithLayer && mCollider2Ds.Count > 0)
        {
            isTriggeredWithLayer = true;
            lastEnterTransform = col.transform;
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
