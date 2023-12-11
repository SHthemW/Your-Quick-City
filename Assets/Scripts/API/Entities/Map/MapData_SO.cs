using UnityEngine;
using UnityEngine.Serialization;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Map", menuName = "Data/Map")]
    public sealed class MapData_SO : ScriptableObject, IMapData
    {
        [Space, SerializeField]
        private MapProperty _properties;

        [Space, SerializeField]
        private MapEntities _gameObjects;

        /*
         *  implements
         */

        MapProperty IMapData.Properties => _properties;
        MapEntities IMapData.GameObjectDef => _gameObjects;
    }
}
