using System;
using System.Collections;
using UnityEngine;
using Yours.QuickCity.Internal;

namespace Yours.QuickCity
{
    public sealed class Map
    {
        private readonly IMapData      _data;
        private readonly IMapConf      _conf;

        private readonly IMapObjParent _parent;
        private readonly MonoBehaviour _master;

        private MapDiagram            _diagram;
        private IMapTerrainDetector[] _terrainDetectors;

        public Map(IMapData data, IMapConf conf, IMapObjParent parent, MonoBehaviour master)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _conf = conf ?? throw new ArgumentNullException(nameof(conf));
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _master = master != null ? master : throw new ArgumentNullException(nameof(master));
        }
        public void Generate()
        {
            _master.StartCoroutine(GenerateSeqence());
        }

        private Map() 
            => throw new NotImplementedException();
        private IEnumerator GenerateSeqence()
        {
            _diagram = new(_data.BasicProperty);

            LogUI.AppendLog("start gen buildings..");
            yield return _master.StartCoroutine(GenerateBuildingsOnMap(_diagram));

            LogUI.AppendLog("start gen detectors..");
            yield return _master.StartCoroutine(GenerateDetectorsOnMap(_diagram));

            LogUI.AppendLog("start parse datas..");
            yield return _master.StartCoroutine(GenerateStuffByTerrain(_terrainDetectors));

            LogUI.AppendLog("generate finished.");
        }
        private IEnumerator GenerateBuildingsOnMap(MapDiagram map)
        {
            new MapBldgBaseDiagramGenerator(_data.BasicProperty, _data.BuildingGenerationProperty).GenerateOnDiagram(map);

            new MapBldgStructureDiagramGenerator(_data.StructureGenerationProperty).GenerateOnDiagram(map);

            map.PrintDebugGraph();

            var entityGenerator = new MapBldgEntityGenerator(_data.BasicProperty, _data.BuildingGenerationProperty, _parent);
            entityGenerator.GenerateByDiagram(map);

            yield return new WaitUntil(entityGenerator.GenerateIsFinished);
        }
        private IEnumerator GenerateDetectorsOnMap(MapDiagram map)
        {
            var coords = new MapTileCoordsGenerator(_data.BasicProperty, _data.StuffGenerationProperty).GenerateCoords(map);

            var detectorsGenerator = new MapTerrainDetectorGenerator(
                _data.BasicProperty, _data.StuffGenerationProperty, _conf.UtilObjectConf, _parent);

            _terrainDetectors = detectorsGenerator.GenerateDetectors(coords);

            Debug.Log("detectors count: " + _terrainDetectors.Length);

            yield return new WaitUntil(detectorsGenerator.GenerateIsFinished);
        }
        private IEnumerator GenerateStuffByTerrain(IMapTerrainDetector[] terrain)
        {
            var dataAnalyzer = new MapStuffDataAnalyzer(_data.StuffGenerationProperty);

            LogUI.AppendLog("start analysis");

            var stuffObjData = dataAnalyzer.Analysis(terrain);

            yield return new WaitUntil(dataAnalyzer.Finished);

            LogUI.AppendLog("finish analysis");

            dataAnalyzer.PrintDistributionDiagram();

            new MapStuffEntityGenerator(_parent.GetStuffObjParent())
                .GenerateStuffs(stuffObjData);

            yield break;
        }
    }
}
