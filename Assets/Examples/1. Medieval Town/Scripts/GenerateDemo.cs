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

        private IMapObjParent Parent => GetComponent<IMapObjParent>();
        private IDebugLogger LogUI => FindObjectOfType<LogUIDemo>();

        public void Run()
        {
            var map = new Map(_map, Parent, this)
            {
                Logger = this.LogUI
            };

            map.Generate();
        }

        public void Clean()
        {
            // clean objects
            CleanChild(Parent.StuffObjParent);
            CleanChild(Parent.ObstacleObjParent);
            CleanChild(Parent.TerrainDetectorParent);

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
