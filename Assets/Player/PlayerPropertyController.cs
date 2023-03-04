using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    /// <summary>
    /// 这个组件对外提供修改真正玩家属性的API
    /// 必须继承CharacterProperty和IBeAttacked，需要对AttackBox提供玩家敌人一致接口
    /// </summary>
    public class PlayerPropertyController : CharacterProperty, IBeAttacked
    {
        public bool CanRoll => property.NLJS能量减少(property.XHNL消耗能量_Roll);
        public bool GetHurt { get; set; }
        [NaughtyAttributes.Expandable][SerializeField] PlayerProperty property;
        private Dictionary<string, Coroutine> DecreaseHPCors = new Dictionary<string, Coroutine>();

        //真实攻击力
        public override int Attack//该变量只在AttackBox中被调用，需要修改值就直接获取PlayerProperty进行修改
        {
            get
            {
                //连击数加一
                // property.DQLJS当前连击数 += 1;
                // StartDQS清空连击数();
                //能量增加
                //property.NLTS能量提升();
                //TODO根据吸血率回血
                var attack = property.Attack * property.BL攻击增长倍率;
                property.IncreaseHP((int)(property.吸血率 * attack / 100));
                return (int)attack;
            }
            protected set => property.Attack = value;
        }

        protected override void OnEnable() { }

        /// <summary>
        /// 持续扣血
        /// </summary>
        /// <param name="Key">启动扣血协程唯一标识符</param>
        /// <param name="HPPerSec">每秒扣除血量</param>
        public void ContinueDecreaseHP(string Key, int HPPerSec, System.Action action)
        {
            if (!DecreaseHPCors.ContainsKey(Key))
            {
                DecreaseHPCors.Add(Key, StartCoroutine(DecreaseHPTool(HPPerSec, action)));
            }
        }

        /// <summary>
        /// 停止持续扣血
        /// </summary>
        /// /// <param name="Key">扣血协程唯一标识符</param>
        public void StopDecreaseHP(string Key)
        {
            if (DecreaseHPCors.ContainsKey(Key))
            {
                DecreaseHPCors.Remove(Key, out Coroutine value);
                StopCoroutine(value);
            }
        }

        public override void BeAttacked(int damage)
        {
            if (property.BeAttacked(damage))
            {
                //死掉
                if (DQS清空连击数Coroutine != null)
                    StopCoroutine(DQS清空连击数Coroutine);
                DQS清空连击数Coroutine = null;
                property.DQLJS当前连击数 = 0;
            }
            GetHurt = true;
            //Debug.Log($"玩家受伤，当前血量{property.HP}");
        }

        public void StartDQS清空连击数()
        {
            if (!gameObject.activeSelf) return;

            if (DQS清空连击数Coroutine != null)
                StopCoroutine(DQS清空连击数Coroutine);
            DQS清空连击数Coroutine = StartCoroutine(nameof(DQS倒计时清空连击数));
        }

        private WaitForSeconds WaitForCD清空连击数时间;
        private WaitForSeconds waitForOneSecond;
        private Coroutine DQS清空连击数Coroutine;

        private void Awake()
        {
            WaitForCD清空连击数时间 = new WaitForSeconds(property.CD清空连击数时间);
            waitForOneSecond = new WaitForSeconds(1);
            property.ResetProperty();
            property.Add加载默认连击阶段模块();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            DQS清空连击数Coroutine = null;
        }

        private void OnDestroy()
        {
            property.Remove卸载默认连击阶段模块();
            StopAllCoroutines();
            DQS清空连击数Coroutine = null;
        }

        IEnumerator DecreaseHPTool(int HPPerSec, System.Action action)
        {
            while (true)
            {
                if (property.BeAttacked(HPPerSec))
                {
                    //死掉
                    if (DQS清空连击数Coroutine != null)
                        StopCoroutine(DQS清空连击数Coroutine);
                    DQS清空连击数Coroutine = null;
                    property.DQLJS当前连击数 = 0;
                }

                action?.Invoke();

                yield return waitForOneSecond;
            }
        }

        IEnumerator DQS倒计时清空连击数()
        {
            //Debug.Log($"开始清零");
            yield return WaitForCD清空连击数时间;
            //Debug.Log($"完成清零");
            property.DQLJS当前连击数 = 0;
            DQS清空连击数Coroutine = null;
        }
    }
}
