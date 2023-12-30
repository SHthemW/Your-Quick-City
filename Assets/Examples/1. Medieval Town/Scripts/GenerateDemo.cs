using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Yours.QuickCity.Shape;

namespace Yours.QuickCity.Internal
{
    internal sealed class GenerateDemo : MonoBehaviour
    {
        /*
         *  Inspectors
         */

        [SerializeField]
        private MapData_SO _map;

        [SerializeField]
        private Shape_SO[] _shapes; 

        private IMapObjParent Parent => GetComponent<IMapObjParent>();
        private IDebugLogger LogUI => FindObjectOfType<LogUIDemo>();

        public void Run()
        {
            var newProp = CloneAndModify(_map.Properties, new Dictionary<string, object>
            {
                { "_shape", _shapes[1] }
            });

            var newMap = CloneAndModify(_map, new Dictionary<string, object>
            {
                { "_properties", newProp }
            });

            var map = new Map(newMap, Parent, this)
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
        private static T CloneAndModify<T>(T original, Dictionary<string, object> modifications)
        {
            object boxed = original; // Boxing

            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (modifications.TryGetValue(field.Name, out object newValue))
                {
                    field.SetValue(boxed, newValue);
                }
            }

            return (T)boxed; // Unboxing
        }
    }
}
