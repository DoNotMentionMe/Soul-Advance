using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class LDtkLevel_Entrance : LDtkLevel
    {
        private PlayerFSM Player;

        public override void ClearLevel()
        {
            base.ClearLevel();
            Player.gameObject.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Player.gameObject.SetActive(true);
        }

        protected override void Awake()
        {
            base.Awake();
            Player = GetComponentInChildren<PlayerFSM>();
        }
    }
}
