using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Map Config", menuName = "Config/Map")]
    internal sealed class MapConf_SO : ScriptableObject, IMapConf
    {
        [SerializeField]
        private MapUtilObjectConf _utilObjectConf;

        /*
         *  implements
         */

        public MapUtilObjectConf UtilObjectConf => _utilObjectConf;
    }
}
