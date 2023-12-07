using System.Collections;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapCtrller : MonoBehaviour
    {
        /*
         *  Inspectors
         */
       
        [Header("Properties")]

        [SerializeField]
        private MapData_SO _map;

        [SerializeField]
        private MapConf_SO _conf;

        private void Start()
        {
            var map = new Map(_map, _conf, GetComponent<IMapObjParent>(), this);
            map.Generate();
        }
    }
}
