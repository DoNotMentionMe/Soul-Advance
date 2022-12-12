using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class EnemyGeneratedData : MonoBehaviour
    {
        public float Occupation => occupation;
        public float GeneratedOffsetY => generatedOffsetY;
        [SerializeField] float occupation;
        [SerializeField] float generatedOffsetY;

        /// <summary>
        /// 获取敌人所在区域
        /// </summary>
        /// <param name="region"></param>
        public void GetRegion(EnemyGenerationRegion region)
        {
            selfRegion = region;
            regionIsNull = false;
        }

        private EnemyGenerationRegion selfRegion;
        private bool regionIsNull = true;

        private void OnDisable()
        {
            //释放占用和区域
            if (!regionIsNull)
            {
                selfRegion.ReleaseOccupation(occupation);
                selfRegion = null;
                regionIsNull = true;
                //测试
                EnemyGenerator.Instance.startGenerate.Invoke();
            }


        }
    }
}