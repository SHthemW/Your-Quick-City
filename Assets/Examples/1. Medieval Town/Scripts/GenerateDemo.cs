using System.Collections;
using System.Linq;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class GenerateDemo : MonoBehaviour
    {
        /*
         *  Inspectors
         */

        [SerializeField]
        private MapData_SO _map;

        public void Run()
        {
            var map = new Map(_map, GetComponent<IMapObjParent>(), this);
            map.Generate();
        }

        public void Clean()
        {
            // clean objects
            var handler = GetComponent<IMapObjParent>();
            CleanChild(handler.StuffObjParent);
            CleanChild(handler.ObstacleObjParent);
            CleanChild(handler.TerrainDetectorParent);

            // clean ui
            LogUI.Clear();
        }

        private static void CleanChild(Transform parent)
        {
            if (parent == null) 
                return;

            while (parent.childCount > 0)
            {
                var child = parent.GetChild(0);
                UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
        }
    }
}
