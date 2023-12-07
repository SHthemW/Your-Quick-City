using UnityEngine;

namespace Yours.QuickCity.Internal
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
