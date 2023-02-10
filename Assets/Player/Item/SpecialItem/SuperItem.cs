using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    //[CreateAssetMenu(menuName = ("Data/PlayerItem/SuperItem"), fileName = ("SuperItem"))]
    public abstract class SuperItem : ScriptableObject
    {
        public int MaxLevel = 1;
        public int CurrentLevel = 0;
        protected virtual void OnEnable()
        {
            CurrentLevel = 0;
        }
        public virtual void ZZ装载() => CurrentLevel = 1;
        public virtual void XZ卸载() => CurrentLevel = 0;
        public virtual void SJ升级()
        {
            if (CurrentLevel < MaxLevel)
                CurrentLevel += 1;
        }
        /// <summary>
        /// CurrentLevel等于1时，返回true
        /// </summary>
        /// <returns></returns>
        public virtual bool JJ降级()
        {
            if (CurrentLevel > 1)
            {
                CurrentLevel -= 1;
                return false;
            }
            return true;
        }
        public abstract void Effect();
        public abstract void EffectRemove();

    }
}
