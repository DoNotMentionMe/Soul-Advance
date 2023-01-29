using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class EnemyGeneratedData : MonoBehaviour
    {
        public EnemyGenerate SelfRegion => selfRegion;
        //public float Occupation => occupation;
        public float GeneratedOffsetY => generatedOffsetY;
        public bool RegionIsNull => regionIsNull;
        public event UnityAction<EnemyGeneratedData> onDied = delegate { };//对象池调用，用于将对象放回对象池
        //[SerializeField] float occupation=1;
        [SerializeField] float generatedOffsetY;

        /// <summary>
        /// 获取敌人所在区域
        /// </summary>
        /// <param name="region"></param>
        public bool GetRegion(EnemyGenerate region)
        {
            selfRegion = region;
            regionIsNull = false;
            return true;
        }

        /// <summary>
        /// 拖拽到EnemyProperty组件的DiedEvent中
        /// </summary>
        public void OnDied()
        {
            //释放占用和区域
            if (!regionIsNull)
            {
                selfRegion.ReleaseOccupation(this);
                selfRegion = null;
                regionIsNull = true;
            }

            onDied?.Invoke(this);
            gameObject.SetActive(false);
        }

        private EnemyGenerate selfRegion;
        private bool regionIsNull = true;

        private void OnDisable()
        {

        }

        private void OnDestroy()
        {
            onDied = null;
        }

    }
}
