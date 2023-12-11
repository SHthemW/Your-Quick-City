using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Map", menuName = "Data/Map")]
    public sealed class MapData_SO : ScriptableObject, IMapData
    {
        [SerializeField]
        private MapConf_SO _config;

        [Space, SerializeField]
        private MapProperty _properties;

        [Space, SerializeField]
        private MapEntities _gameObjects;

        /*
         *  implements
         */

        IMapConf    IMapData.Config => _config != null ? _config : throw new MissingReferenceException();
        MapProperty IMapData.Properties => _properties;
        MapEntities IMapData.GameObjectDef => _gameObjects;
    }
}
