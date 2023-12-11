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
            _diagram = new(_data.Properties);

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
            new MapBldgBaseDiagramGenerator(_data.Properties, _data.GameObjectDef).GenerateOnDiagram(map);

            var structureGenerator = new MapBldgStructureDiagramGenerator(_data.GameObjectDef);
            structureGenerator.GenerateOnDiagram(map);

            if (_data.Properties.PrintMapGridDiagram)
                map.PrintDebugGraph();

            if (_data.Properties.ShowStructureGenerateResult)
                structureGenerator.PrintGenerateResult();

            var entityGenerator = new MapBldgEntityGenerator(_data.Properties, _data.GameObjectDef, _parent);
            entityGenerator.GenerateByDiagram(map);

            yield return new WaitUntil(entityGenerator.GenerateIsFinished);
        }
        private IEnumerator GenerateDetectorsOnMap(MapDiagram map)
        {
            var coords = new MapTileCoordsGenerator(_data.Properties).GenerateCoords(map);

            var detectorsGenerator = new MapTerrainDetectorGenerator(
                _data.Properties, _conf.UtilObjectConf, _parent);

            _terrainDetectors = detectorsGenerator.GenerateDetectors(coords);

            yield return new WaitUntil(detectorsGenerator.GenerateIsFinished);
        }
        private IEnumerator GenerateStuffByTerrain(IMapTerrainDetector[] terrain)
        {
            var dataAnalyzer = new MapStuffDataAnalyzer(_data.GameObjectDef, _data.Properties);

            LogUI.AppendLog("start analysis");

            var stuffObjData = dataAnalyzer.Analysis(terrain);

            yield return new WaitUntil(dataAnalyzer.Finished);

            LogUI.AppendLog("finish analysis");

            if (_data.Properties.ShowStuffDistributionInfo)
                dataAnalyzer.PrintDistributionDiagram();

            new MapStuffEntityGenerator(_parent.StuffObjParent)
                .GenerateStuffs(stuffObjData);

            yield break;
        }
    }
}
