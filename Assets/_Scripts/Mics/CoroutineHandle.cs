using System;
using System.Collections;
using UnityEngine;

public class CoroutineHandle : IEnumerator
{
    public event Action<CoroutineHandle> OnCompleted;
    public bool IsDone { get; private set; }
    public bool MoveNext() => !IsDone;
    public object Current { get; }
    public void Reset() { }

    public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine)
    {
        Current = owner.StartCoroutine(Wrap(coroutine));
    }

    private IEnumerator Wrap(IEnumerator coroutine)
    {
        yield return coroutine;
        IsDone = true;
        OnCompleted?.Invoke(this);
    }
}

public static class MonoBehaviourExtensions
{
    public static CoroutineHandle RunCoroutine(
        this MonoBehaviour owner,
        IEnumerator coroutine
    )
    {
        return new CoroutineHandle(owner, coroutine);
    }
}
