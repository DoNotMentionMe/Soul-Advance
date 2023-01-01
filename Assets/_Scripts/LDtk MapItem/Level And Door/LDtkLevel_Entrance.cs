using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LDtkUnity;

namespace Adv
{
    public class LDtkLevel_Entrance : LDtkLevel, ILDtkImportedLevel
    {
        [SerializeField] PlayerFSM Player;

        public override void ShowLevel()
        {

        }

        public override void HideLevel()
        {

        }

        public override void ClearLevel()
        {
            base.ClearLevel();
            Player.gameObject.SetActive(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Camera.main.transform.position = Player.transform.position;
            Player.gameObject.SetActive(true);
        }

        public void OnLDtkImportLevel(Level level)
        {
            Player = GetComponentInChildren<PlayerFSM>();
        }
    }
}
