using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/SetPosprocessLevel"), fileName = ("SetPosprocessLevel"))]
    public class SetPosprocessLevel : ScriptableObject
    {
        [SerializeField] Material UnityDefaultUnlit;
        [SerializeField] int GroundOrderInLayer;
        [SerializeField] LayerMask OneWayPlatformer;
        [SerializeField] float OneWayEffectorSurfaceArc = 130;
        private const string GroundLayerName = "Ground";

        /// <summary>
        /// 设置层级关系
        /// </summary>
        /// <param name="renderer"></param>
        public void SetRenderer(Renderer renderer)
        {
            renderer.material = UnityDefaultUnlit;
            renderer.sortingLayerName = GroundLayerName;
            renderer.sortingOrder = GroundOrderInLayer;

        }

        /// <summary>
        /// 找到单向平台层，添加组件并设置好参数
        /// </summary>
        public bool AddOneWayEffector(GameObject Grid)
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
                return true;
            }
            return false;
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
