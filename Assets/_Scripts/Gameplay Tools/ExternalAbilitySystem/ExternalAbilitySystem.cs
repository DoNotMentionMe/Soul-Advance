using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Adv
{
    public class ExternalAbilitySystem : PersistentSingleton<ExternalAbilitySystem>
    {
        public bool IsEffected { get; set; }
        [SerializeField] List<SuperItem> LearnedItems = new List<SuperItem>();

        /// <summary>
        /// 局外功能，装载或升级
        /// </summary>
        public void ZZ装载道具(SuperItem Item)
        {
            if (LearnedItems.Contains(Item))
            {
                Item.SJ升级();
                return;
            }
            LearnedItems.Add(Item);
            Item.ZZ装载();
            Debug.Log($"装载道具{Item.name}");
        }

        /// <summary>
        /// 局外功能，降级或卸载
        /// </summary>
        /// <param name="Item"></param>
        public void XZ卸载道具(SuperItem Item)
        {
            if (!LearnedItems.Contains(Item))
                return;
            else if (Item.JJ降级())
            {
                LearnedItems.Remove(Item);
                Item.XZ卸载();
                Debug.Log($"卸载道具{Item.name}");
            }
        }

        public void XZA卸载所有道具()
        {
            LearnedItems.RemoveAll((item => true));
        }

        /// <summary>
        /// 控制游戏流程
        /// </summary>
        public void EffectAllItem()
        {
            foreach (var item in LearnedItems)
            {
                item.Effect();
            }
            IsEffected = true;
        }

        public void RemoveAllItem()
        {
            foreach (var item in LearnedItems)
            {
                item.EffectRemove();
            }
            IsEffected = false;
        }
    }
}
