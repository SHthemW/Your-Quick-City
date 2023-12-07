using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Map", menuName = "Data/Map")]
    internal sealed class MapData_SO : ScriptableObject, IMap
    {
        [Space, SerializeField]
        private MapBasicProperty _basicProperty;
    
        [Space, SerializeField]
        private MapBaseGenerationProperty _baseGenerationProperty;

        [Space, SerializeField]
        private MapStructureGenerationProperty _structureGenerationProperty;

        [Space, SerializeField]
        private MapStuffGenerationProperty _stuffGenerationProperty;

        /*
         *  implements
         */

        public MapBasicProperty BasicProperty => _basicProperty;
        public MapBaseGenerationProperty BaseGenerationProperty => _baseGenerationProperty;
        public MapStructureGenerationProperty StructureGenerationProperty => _structureGenerationProperty;
        public MapStuffGenerationProperty StuffGenerationProperty => _stuffGenerationProperty;
    }
}
