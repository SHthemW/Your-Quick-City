using UnityEngine;
using UnityEngine.Serialization;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Map", menuName = "Data/Map")]
    internal sealed class MapData_SO : ScriptableObject, IMap
    {
        [Space, SerializeField]
        private MapSizeProperty _size;

        [Space, SerializeField]
        private MapBuildingGenerationProperty _buildingGenerationProperty;

        [Space, SerializeField]
        private MapStructureGenerationProperty _structureGenerationProperty;

        [Space, SerializeField]
        private MapStuffGenerationProperty _stuffGenerationProperty;

        /*
         *  implements
         */

        public MapSizeProperty BasicProperty => _size;
        public MapBuildingGenerationProperty  BuildingGenerationProperty  => _buildingGenerationProperty;
        public MapStructureGenerationProperty StructureGenerationProperty => _structureGenerationProperty;
        public MapStuffGenerationProperty     StuffGenerationProperty     => _stuffGenerationProperty;
    }
}
