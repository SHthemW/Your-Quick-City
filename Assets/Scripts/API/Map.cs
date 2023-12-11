using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            LogUI.AppendLog("generating buildings..");
            yield return _master.StartCoroutine(GenerateBuildingsOnMap(_diagram));
        
            yield return _master.StartCoroutine(GenerateDetectorsOnMap(_diagram));

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
            #region generate coords

            var coordsGenerator = new MapTileCoordsGenerator(_map.Properties);

            LogUI.AppendLog("generating coords..");
            LogUI.AppendDynamicPercent(coordsGenerator.FinishedPercent);

            _master.StartCoroutine    (coordsGenerator.GenerateCoords(map));
            yield return new WaitUntil(coordsGenerator.Completed);
            var coords =               coordsGenerator.Result.ToArray();

            LogUI.EndDynamicPart(true);

            #endregion

            #region generate detectors

            var detectorsGenerator = new MapTerrainDetectorGenerator(_map.Properties, _map.Config.TerrainDetector, _parent);

            LogUI.AppendLog("generating detectors..");
            LogUI.AppendDynamicPercent(detectorsGenerator.FinishedPercent);

            _master.StartCoroutine    (detectorsGenerator.GenerateDetectors(coords));
            yield return new WaitUntil(detectorsGenerator.Completed);
            _terrainDetectors =        detectorsGenerator.Result.ToArray();

            LogUI.EndDynamicPart();

            #endregion
        }
        private IEnumerator GenerateStuffByTerrain(MapTerrainDetector[] terrain)
        {
            #region generate stuff distribution

            var distDiagGenerator = new MapStuffDistributionDiagramGenerator(_map.GameObjectDef, _map.Properties);

            LogUI.AppendLog("generating stuff dist...");
            LogUI.AppendDynamicPercent(distDiagGenerator.FinishedPercent);

            _master.StartCoroutine    (distDiagGenerator.BakeDistribution(maxDensity: _terrainDetectors.Max(d => d.DensityValue)));
            yield return new WaitUntil(distDiagGenerator.Completed);
            var distribution =         distDiagGenerator.Result;

            LogUI.EndDynamicPart(true);

            if (_map.Config.ShowStuffDistributionInfo)
                distDiagGenerator.PrintDistributionDiagram();

            #endregion

            #region analyze stuff distribution

            var dataAnalyzer = new MapStuffDataAnalyzer();

            LogUI.AppendLog("analysing stuff dist...");
            LogUI.AppendDynamicPercent(dataAnalyzer.FinishedPercent);

            _master.StartCoroutine    (dataAnalyzer.Analysis(terrain, distribution));
            yield return new WaitUntil(dataAnalyzer.Completed);
            var stuffObjData =         dataAnalyzer.Result;

            LogUI.EndDynamicPart(true);

            #endregion

            #region generate stuff gameobjects 

            var stuffEntityGenerator = new MapStuffEntityGenerator(_parent.StuffObjParent);

            LogUI.AppendLog("generating stuffs...");
            LogUI.AppendDynamicPercent(dataAnalyzer.FinishedPercent);

            _master.StartCoroutine    (stuffEntityGenerator.GenerateStuffs(stuffObjData));
            yield return new WaitUntil(stuffEntityGenerator.Completed);

            LogUI.EndDynamicPart();

            #endregion

            yield break;
        }
    }
}
