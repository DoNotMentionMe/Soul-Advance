using System.Collections;
using System.Collections.Generic;
using LDtkUnity;
using LDtkUnity.Editor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Adv
{
    public class AdvPostprocessor : LDtkPostprocessor
    {
        // protected override void OnPostprocessProject(GameObject root)
        // {
        //     Debug.Log($"Post process LDtk project: {root.name}");
        // }

        protected override void OnPostprocessLevel(GameObject root, LdtkJson projectJson)
        {
            Debug.Log($"Post process LDtk level: {root.name}");
            Resources.Load<SetPosprocessLevel>("SetPosprocessLevel").SetRenderer(root.GetComponentInChildren<TilemapRenderer>());
        }
    }
}
