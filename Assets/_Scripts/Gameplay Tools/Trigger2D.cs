using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger2D : MonoBehaviour
{
    public bool IsTriggered => isTriggeredWithLayer;
    public float Length => ((BoxCollider2D)mCol).size.x;//只有PlayerController用到，必须是BOXCOLL
    public Vector2 Pos => mTransform.position;

    [SerializeField] bool isTriggeredWithLayer;
    [Tooltip("只在触发那一帧执行触发时间")]
    [SerializeField] bool OnlyInvokeEventWhenTriggerFrame = false;
    [SerializeField] LayerMask layers;
    [SerializeField] Transform mTransform;
    [SerializeField] UnityEvent OnTriggerEnter = new UnityEvent();
    // public UnityEvent OnTriggerExit = new UnityEvent();
    [SerializeField] UnityEvent<Collider2D> OnTriggerEnterWithCollider = new UnityEvent<Collider2D>();
    [SerializeField] UnityEvent OnSetCollFalse = new UnityEvent();
    //public UnityEvent<Collider2D> OnTriggerExitWithCollider = new UnityEvent<Collider2D>();

    private Collider2D mCol;

    private HashSet<Collider2D> mCollider2Ds = new HashSet<Collider2D>();

    private void Awake()
    {
        mCol = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        //SetCollEnable(true);
    }

    private void OnDisable()
    {
        mCollider2Ds.Clear();
        isTriggeredWithLayer = false;
    }

    private void OnDestroy()
    {
        mCol = null;
        mCollider2Ds.Clear();
        mCollider2Ds = null;
        isTriggeredWithLayer = false;
    }

    public void ResetTrigger()
    {
        SetCollEnable(false);
        mCollider2Ds.Clear();
        isTriggeredWithLayer = false;
    }

    /// <summary>
    /// 玩家的每次攻击状态结束后调用，代表单次攻击的结束
    /// </summary>
    public void SetCollEnable(bool enable)
    {
        mCol.enabled = enable;
        if (!enable)
        {
            OnSetCollFalse?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!LayerMaskUtility.Contains(layers, col.gameObject.layer)) return;

        if (!mCollider2Ds.Contains(col))
            mCollider2Ds.Add(col);
        if (!OnlyInvokeEventWhenTriggerFrame)
        {
            OnTriggerEnter?.Invoke();
            OnTriggerEnterWithCollider?.Invoke(col);
        }

        if (!isTriggeredWithLayer && mCollider2Ds.Count > 0)
        {
            if (OnlyInvokeEventWhenTriggerFrame)
            {
                OnTriggerEnter?.Invoke();
                OnTriggerEnterWithCollider?.Invoke(col);
            }
            isTriggeredWithLayer = true;
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
