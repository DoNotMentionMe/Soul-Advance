using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/SetPosprocessLevel"), fileName = ("SetPosprocessLevel"))]
    public class SetPosprocessLevel : ScriptableObject
    {
        [SerializeField] Material UnityDefaultUnlit;
        [SerializeField] int GroundOrderInLayer;
        [SerializeField] LayerMask Ground;
        [SerializeField] LayerMask OneWayPlatformer;
        [SerializeField] float OneWayEffectorSurfaceArc = 130;
        [SortingLayer][SerializeField] string GroundSortingLayerName = "Ground";
        [Layer][SerializeField] int LDtkLevelLayerName;
        [Tag][SerializeField] string LDtkLevelTagName;
        [SerializeField] LDtkLevelEventChannel PlayerEnterLevel;
        [SerializeField] LDtkLevelEventChannel PlayerLeaveLevel;

        public void SetLDtkLevel(LDtkLevel level)
        {
            level.PlayerEnterLevel = PlayerEnterLevel;
            level.PlayerLeaveLevel = PlayerLeaveLevel;
        }

        /// <summary>
        /// 设置地图块对象的Layer为LDtkLevel
        /// </summary>
        public void SetLDtkLevelLayerAndTag(GameObject level)
        {
            level.tag = LDtkLevelTagName;
            level.layer = LDtkLevelLayerName;
        }

        /// <summary>
        /// 设置层级关系
        /// </summary>
        /// <param name="renderer"></param>
        public void SetRenderer(Renderer renderer)
        {
            renderer.material = UnityDefaultUnlit;
            renderer.sortingLayerName = GroundSortingLayerName;
            renderer.sortingOrder = GroundOrderInLayer;

        }

        /// <summary>
        /// 找到普通地面层，添加组件并设置好参数
        /// </summary>
        public void SetGround(GameObject Grid)
        {
            if (LayerMaskUtility.Contains(Ground, Grid.layer))
            {
                var coll = Grid.GetComponent<CompositeCollider2D>();
                coll.geometryType = CompositeCollider2D.GeometryType.Polygons;
            }
        }

        /// <summary>
        /// 找到单向平台层，添加组件并设置好参数
        /// </summary>
        public void AddOneWayEffector(GameObject Grid)
        {
            if (LayerMaskUtility.Contains(OneWayPlatformer, Grid.layer))
            {
                var coll = Grid.GetComponent<CompositeCollider2D>();
                coll.usedByEffector = true;
                coll.geometryType = CompositeCollider2D.GeometryType.Polygons;
                var effector = Grid.AddComponent<PlatformEffector2D>();
                effector.useColliderMask = false;
                effector.useOneWayGrouping = true;
                effector.surfaceArc = OneWayEffectorSurfaceArc;
            }
        }

        public void ResetLevelTrigger(PolygonCollider2D trigger)
        {
            //设置顺序：左上-左下-右下-右上
            var points = trigger.GetPath(0);
            points[0].x += 1;
            points[0].y -= 1;
            points[1].x += 1;
            points[1].y += 1;
            points[2].x -= 1;
            points[2].y += 1;
            points[3].x -= 1;
            points[3].y -= 1;
            trigger.SetPath(0, points);
        }

    }
}
