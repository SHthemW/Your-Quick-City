using System.Collections;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapGenerator : MonoBehaviour
    {
        /*
         *  Inspectors
         */

        [SerializeField]
        private MapData_SO _map;

        private void Start()
        {
            var map = new Map(_map, GetComponent<IMapObjParent>(), this);
            map.Generate();
        }
    }
}
