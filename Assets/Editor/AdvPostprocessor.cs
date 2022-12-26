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

        /// <summary>
        /// 地图块加载时执行
        /// </summary>
        /// <param name="root">地图块根对象</param>
        protected override void OnPostprocessLevel(GameObject root, LdtkJson projectJson)
        {
            //Debug根对象名
            Debug.Log($"Post process LDtk level: {root.name}");

            var setting = Resources.Load<SetPosprocessLevel>("SetPosprocessLevel");

            //根对象Trigger四个叫缩小一个单位
            setting.ResetLevelTrigger(root.GetComponent<PolygonCollider2D>());
            //把地图块的排序图层改为对应位置
            setting.SetRenderer(root.GetComponentInChildren<TilemapRenderer>());
            //如果存在单向平台
            foreach (Transform child in root.transform)
            {
                if (child.GetComponent<LDtkComponentLayer>().LayerType == TypeEnum.IntGrid)
                {
                    foreach (Transform childChild in child)
                    {
                        setting.AddOneWayEffector(childChild.gameObject);
                    }
                }
            }

            setting = null;
        }
    }
}
