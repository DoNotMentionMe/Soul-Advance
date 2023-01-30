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
        public bool CanRoll => property.NLJS能量减少(XHNL_Roll);
        public bool GetHurt { get; set; }
        [SerializeField] float CD清空连击数时间 = 3;
        [SerializeField] int XHNL_Roll = 25;
        [NaughtyAttributes.Expandable][SerializeField] PlayerProperty property;

        //真实攻击力
        public override int Attack//该变量只在AttackBox中被调用，需要修改值就直接获取PlayerProperty进行修改
        {
            get
            {
                //连击数加一
                property.DQLJS当前连击数 += 1;
                StartDQS清空连击数();
                //能量增加
                property.NLTS能量提升();
                return (int)(property.Attack * property.BL攻击增长倍率);
            }
            protected set => property.Attack = value;
        }

        protected override void OnEnable() { }

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
            if (DQS清空连击数Coroutine != null)
                StopCoroutine(DQS清空连击数Coroutine);
            DQS清空连击数Coroutine = StartCoroutine(nameof(DQS倒计时清空连击数));
        }

        private WaitForSeconds WaitForCD清空连击数时间;
        private Coroutine DQS清空连击数Coroutine;

        private void Awake()
        {
            WaitForCD清空连击数时间 = new WaitForSeconds(CD清空连击数时间);
        }

        IEnumerator DQS倒计时清空连击数()
        {
            Debug.Log($"开始清零");
            yield return WaitForCD清空连击数时间;
            Debug.Log($"完成清零");
            property.DQLJS当前连击数 = 0;
            DQS清空连击数Coroutine = null;
        }
    }
}
