using System;
using System.Collections;
using UnityEngine;

public class CoroutineHandle
{
    public bool IsDone { get; private set; }
    public Coroutine Current;
    MonoBehaviour Owner;
    IEnumerator Coroutine;

    public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine)
    {
        Owner = owner;
        Coroutine = coroutine;
    }

    public void RunAgain(bool InterruptCurrent = true)
    {
        if (InterruptCurrent && !IsDone)
        {
            Owner.StopCoroutine(Current);
        }
        Current = Owner.StartCoroutine(Wrap(Coroutine));
    }

    public void StopCurrent()
    {
        if (!IsDone)
        {
            Owner.StopCoroutine(Current);
        }
    }

    private IEnumerator Wrap(IEnumerator coroutine)
    {
        yield return coroutine;
        IsDone = true;
    }
}

public static class MonoBehaviourExtensions
{
    public static CoroutineHandle CreateCoroutineHandle(
        this MonoBehaviour owner,
        IEnumerator coroutine
    )
    {
        return new CoroutineHandle(owner, coroutine);
    }
}

// public class CoroutineHandle : IEnumerator
// {
//     public event Action<CoroutineHandle> OnCompleted;
//     public bool IsDone { get; private set; }
//     public bool MoveNext() => !IsDone;
//     public object Current { get; }
//     public void Reset() { }

//     public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine)
//     {
//         Current = owner.StartCoroutine(Wrap(coroutine));
//     }

//     private IEnumerator Wrap(IEnumerator coroutine)
//     {
//         yield return coroutine;
//         IsDone = true;
//         OnCompleted?.Invoke(this);
//     }
// }

