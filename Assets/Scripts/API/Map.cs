using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yours.QuickCity.Internal;

namespace Yours.QuickCity
{
    public sealed class Map
    {
        private readonly IMapData      _map;

        private readonly IMapObjParent _parent;
        private readonly MonoBehaviour _master;

        private MapDiagram           _diagram;
        private MapTerrainDetector[] _terrainDetectors;

        public Map(IMapData data, IMapObjParent parent, MonoBehaviour master)
        {
            _map    = data ?? throw new ArgumentNullException(nameof(data));
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
            _diagram = new(_map.Properties);

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
            new MapBldgBaseDiagramGenerator(_map.Properties, _map.GameObjectDef).GenerateOnDiagram(map);

            var structureGenerator = new MapBldgStructureDiagramGenerator(_map.GameObjectDef);
            structureGenerator.GenerateOnDiagram(map);

            if (_map.Config.PrintMapGridDiagram)
                map.PrintDebugGraph();

            if (_map.Config.ShowStructureGenerateResult)
                structureGenerator.PrintGenerateResult();

            var entityGenerator = new MapBldgEntityGenerator(_map.Properties, _map.GameObjectDef, _parent);
            entityGenerator.GenerateByDiagram(map);

            yield return new WaitUntil(entityGenerator.GenerateIsFinished);
        }
        private IEnumerator GenerateDetectorsOnMap(MapDiagram map)
        {
            var coords = new MapTileCoordsGenerator(_map.Properties).GenerateCoords(map);

            var detectorsGenerator = new MapTerrainDetectorGenerator(_map.Properties, _map.Config.TerrainDetector, _parent);

            LogUI.AppendDynamicPercent(detectorsGenerator.FinishedPercent);

            _master.StartCoroutine(detectorsGenerator.GenerateDetectors(coords));

            yield return new WaitUntil(detectorsGenerator.Completed);

            _terrainDetectors = detectorsGenerator.Result.ToArray();

            LogUI.EndDynamicPart();
        }
        private IEnumerator GenerateStuffByTerrain(MapTerrainDetector[] terrain)
        {
            var dataAnalyzer = new MapStuffDataAnalyzer(_map.GameObjectDef, _map.Properties);

            LogUI.AppendLog("start analysis");
            LogUI.AppendDynamicPercent(dataAnalyzer.FinishedPercent);

            _master.StartCoroutine(dataAnalyzer.Analysis(terrain));

            yield return new WaitUntil(dataAnalyzer.Completed);

            var stuffObjData = dataAnalyzer.Result;

            LogUI.EndDynamicPart();
            LogUI.AppendLog("finish analysis");

            if (_map.Config.ShowStuffDistributionInfo)
                dataAnalyzer.PrintDistributionDiagram();

            new MapStuffEntityGenerator(_parent.StuffObjParent)
                .GenerateStuffs(stuffObjData);

            yield break;
        }
    }
}
