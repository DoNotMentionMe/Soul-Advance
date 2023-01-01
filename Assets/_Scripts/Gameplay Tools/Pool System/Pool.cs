using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Adv
{
    /// <summary>
    /// 目前存在对象池不断膨胀的情况，会造成多余内存占用
    /// </summary>
    [System.Serializable]//未继承MonoBehaviour类，需要开头添加此字段才能将序列化字段曝露出去
    public class Pool
    {
        // public GameObject Prefab
        // {
        //     get
        //     {
        //         return prefab;
        //     }
        // }

        public GameObject Prefab { get => prefab; set => prefab = value; }

        //public GameObject Prefab { get => prefab};
        public int Size { get => size; set => size = value; }
        public int RuntimeSize => queue.Count;

        [SerializeField] GameObject prefab;
        [SerializeField] int size = 1;
        Queue<GameObject> queue;//跟列表一样，但是有一些特别的函数

        Transform parent;

        public void Initialize(Transform parent)
        {
            queue = new Queue<GameObject>();
            this.parent = parent;
            for (var i = 0; i < size; i++)
            {
                queue.Enqueue(Copy());
            }
        }

        /// <summary>
        /// 用于初始化时的实例化
        /// </summary>
        GameObject Copy()
        {
            var copy = GameObject.Instantiate(prefab, parent);

            copy.SetActive(false);

            return copy;
        }

        /// <summary>
        /// 用于释放对象时的实例化
        /// copy在实例化挂载Feel插件的对象时存在无法自动播放的情况
        /// </summary>
        GameObject Get()
        {
            return GameObject.Instantiate(prefab, parent);
        }

        GameObject AvailableObject()
        {
            GameObject availableObject = null;

            if (queue.Count > 0 && !queue.Peek().activeSelf)//这里检查第一个元素是否为启动状态来解决下面文字说到的问题
            {
                availableObject = queue.Dequeue();
            }
            else
            {
                availableObject = Get();
            }
            //如果数量没有规划好，可能出现列头对象激活状态，后面的对象闲置状态，但是取不出来导致无限生成新的对象的情况
            queue.Enqueue(availableObject);//提前入列，不需要在对象使用后调用一次入列的方法，但是有个弊端，如果对象池调用过快，这个提前入列的会再被调用到新的地方，比如子弹飞一半被重新调用道枪口的位置


            return availableObject;

        }

        public GameObject PreparedObject()
        {
            GameObject preparedObject = AvailableObject();

            preparedObject.SetActive(true);

            return preparedObject;
        }

        public GameObject PreparedObject(Vector3 position)
        {
            GameObject preparedObject = AvailableObject();

            preparedObject.transform.position = position;
            preparedObject.SetActive(true);

            return preparedObject;
        }

        public GameObject PreparedObject(Vector2 positionOffset)
        {
            GameObject preparedObject = AvailableObject();

            preparedObject.transform.position += (Vector3)positionOffset * Mathf.Sign(preparedObject.transform.localScale.x);
            preparedObject.SetActive(true);

            return preparedObject;
        }

        public GameObject PreparedObject(Vector3 position, Quaternion rotation)
        {
            GameObject preparedObject = AvailableObject();

            preparedObject.transform.position = position;
            preparedObject.transform.rotation = rotation;
            preparedObject.SetActive(true);

            return preparedObject;
        }
        public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
        {
            GameObject preparedObject = AvailableObject();

            preparedObject.transform.position = position;
            preparedObject.transform.rotation = rotation;
            preparedObject.transform.localScale = localScale;
            preparedObject.SetActive(true);

            return preparedObject;
        }


        public void Return(GameObject gameObject)
        {
            if (!queue.Contains(gameObject))
                queue.Enqueue(gameObject);
        }
    }
}