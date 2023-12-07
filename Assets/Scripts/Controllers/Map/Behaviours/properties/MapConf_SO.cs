using Game.General.Interfaces;
using Game.General.Properties;
using UnityEngine;

namespace Game.Ctrller.Map
{
    [CreateAssetMenu(fileName = "New Map Config", menuName = "Config/Map")]
    public sealed class MapConf_SO : ScriptableObject, IMapConf
    {
        [SerializeField]
        private MapUtilObjectConf _utilObjectConf;

        /*
         *  implements
         */

        public MapUtilObjectConf UtilObjectConf => _utilObjectConf;
    }
}
