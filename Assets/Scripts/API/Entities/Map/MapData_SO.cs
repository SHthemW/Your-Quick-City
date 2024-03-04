using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Map", menuName = "Map/New")]
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

        public IMapConf    Config => _config != null ? _config : throw new MissingReferenceException();
        public MapProperty Properties => _properties;
        public MapEntities GameObjectDef => _gameObjects;

        /*
         *  valid checks
         */

        private void OnValidate()
        {
            _gameObjects.CheckValid();
        }
    }
}
