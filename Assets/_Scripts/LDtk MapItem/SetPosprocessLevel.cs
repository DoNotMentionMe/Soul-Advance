using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    [CreateAssetMenu(menuName = ("Data/SetPosprocessLevel"), fileName = ("SetPosprocessLevel"))]
    public class SetPosprocessLevel : ScriptableObject
    {
        [SerializeField] Material UnityDefaultUnlit;
        [SerializeField] int OrderInLayer;
        private const string GroundLayerName = "Ground";

        public void SetRenderer(Renderer renderer)
        {
            renderer.material = UnityDefaultUnlit;
            renderer.sortingLayerName = GroundLayerName;
            renderer.sortingOrder = 0;

        }

    }
}
