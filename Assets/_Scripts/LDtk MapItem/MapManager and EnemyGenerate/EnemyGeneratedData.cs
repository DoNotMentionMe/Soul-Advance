using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Adv
{
    public class EnemyGeneratedData : MonoBehaviour
    {
        public EnemyGenerate SelfRegion => selfRegion;
        public float Occupation => occupation;
        public float GeneratedOffsetY => generatedOffsetY;
        public bool RegionIsNull => regionIsNull;
        public event UnityAction<EnemyGeneratedData> onDisbale = delegate { };
        [SerializeField] float occupation;
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

        private EnemyGenerate selfRegion;
        private bool regionIsNull = true;

        private void OnDisable()
        {
            //释放占用和区域
            if (!regionIsNull)
            {
                selfRegion.ReleaseOccupation(this);
                selfRegion = null;
                regionIsNull = true;
            }

            onDisbale?.Invoke(this);
        }

        private void OnDestroy()
        {
            onDisbale = null;
        }

    }
}