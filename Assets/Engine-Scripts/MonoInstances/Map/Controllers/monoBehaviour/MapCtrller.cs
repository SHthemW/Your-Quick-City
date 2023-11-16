using Game.Ctrller.Map;
using Game.General.Interfaces;
using System.Collections;
using UnityEngine;

namespace Game.Instances.Map
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

        /*
         *  Map
         */

        /// <summary>
        /// current map diagram.
        /// </summary>
        /// <remarks>
        /// the diagram is used as a blueprint of the entity map. <br/>
        /// after generated firstly, it may be process by other <br/>
        /// routine for add structures, stuffs, etc.
        /// </remarks>
        private MapDiagram _currentMapDiagram;
        
        private void Start()
        {           
            StartCoroutine(Generate());
        }
        
        private IEnumerator Generate()
        {
            _currentMapDiagram = new(_map.BasicProperty);

            yield return StartCoroutine(GenerateBuildingsOnMap(_currentMapDiagram));
            GenerateStuffsOnMap(_currentMapDiagram);
        }
        private IEnumerator GenerateBuildingsOnMap(MapDiagram map)
        {           
            new MapBldgBaseDiagramGenerator(_map.BasicProperty, _map.BaseGenerationProperty).GenerateOnDiagram(map);

            new MapBldgStructureDiagramGenerator(_map.StructureGenerationProperty).GenerateOnDiagram(map);

            map.PrintDebugGraph();

            var entityGenerator = new MapBldgEntityGenerator(_map.BasicProperty, _map.BaseGenerationProperty, GetComponent<IMapHandler>());
            entityGenerator.GenerateByDiagram(map);

            yield return new WaitUntil(entityGenerator.GenerateIsFinished);
        }
        private void GenerateStuffsOnMap(MapDiagram map)
        {
            var coords = new MapStuffDetectorCoordsGenerator(_map.BasicProperty, _map.StuffGenerationProperty).GenerateStuffCoords(map);

            var detectors = new MapStuffDetectorEntityGenerator(
                _map.BasicProperty, _map.StuffGenerationProperty, _conf.UtilObjectConf, GetComponent<IMapHandler>())
                .GenerateDetectors(coords);

            new MapStuffEntityGenerator(_map.StuffGenerationProperty, GetComponent<IMapHandler>(), this).GenerateStuffs(detectors);
        }

    }
}
