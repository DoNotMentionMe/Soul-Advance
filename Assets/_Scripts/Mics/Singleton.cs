using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Component//Component是任何挂载到游戏对象上的类型的基类
{
    public static T Instance{get; private set;}

    protected virtual void Awake()
    {
        Instance = this as T;
    }


}
