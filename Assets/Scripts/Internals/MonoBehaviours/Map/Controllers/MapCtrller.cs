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

        private IMapTerrainDetector[] _currentMapTerrainDetectors;
        
        private void Start()
        {           
            StartCoroutine(Generate());
        }
        
        private IEnumerator Generate()
        {
            _currentMapDiagram = new(_map.BasicProperty);

            LogUI.AppendLog("start gen buildings..");
            yield return StartCoroutine(GenerateBuildingsOnMap(_currentMapDiagram));

            LogUI.AppendLog("start gen detectors..");
            yield return StartCoroutine(GenerateDetectorsOnMap(_currentMapDiagram));

            LogUI.AppendLog("start parse datas..");
            yield return StartCoroutine(GenerateStuffByTerrain(_currentMapTerrainDetectors));

            LogUI.AppendLog("generate finished.");
        }
        private IEnumerator GenerateBuildingsOnMap(MapDiagram map)
        {           
            new MapBldgBaseDiagramGenerator(_map.BasicProperty, _map.BuildingGenerationProperty).GenerateOnDiagram(map);

            new MapBldgStructureDiagramGenerator(_map.StructureGenerationProperty).GenerateOnDiagram(map);

            map.PrintDebugGraph();

            var entityGenerator = new MapBldgEntityGenerator(_map.BasicProperty, _map.BuildingGenerationProperty, GetComponent<IMapHandler>());
            entityGenerator.GenerateByDiagram(map);

            yield return new WaitUntil(entityGenerator.GenerateIsFinished);
        }
        private IEnumerator GenerateDetectorsOnMap(MapDiagram map)
        {
            var coords = new MapTileCoordsGenerator(_map.BasicProperty, _map.StuffGenerationProperty).GenerateCoords(map);

            var detectorsGenerator = new MapTerrainDetectorGenerator(
                _map.BasicProperty, _map.StuffGenerationProperty, _conf.UtilObjectConf, GetComponent<IMapHandler>());

            _currentMapTerrainDetectors = detectorsGenerator.GenerateDetectors(coords);

            Debug.Log("detectors count: " + _currentMapTerrainDetectors.Length);

            yield return new WaitUntil(detectorsGenerator.GenerateIsFinished);
        }

        private IEnumerator GenerateStuffByTerrain(IMapTerrainDetector[] terrain)
        {
            var dataAnalyzer = new MapStuffDataAnalyzer(_map.StuffGenerationProperty);

            LogUI.AppendLog("start analysis");

            var stuffObjData = dataAnalyzer.Analysis(terrain);

            yield return new WaitUntil(dataAnalyzer.Finished);

            LogUI.AppendLog("finish analysis");

            dataAnalyzer.PrintDistributionDiagram();

            new MapStuffEntityGenerator(GetComponent<IMapHandler>().GetStuffObjParent())
                .GenerateStuffs(stuffObjData);

            yield break;
        }

    }
}
