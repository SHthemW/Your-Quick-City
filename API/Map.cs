using System;
using System.Collections;
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

        private Matrix<MapNodeData>  _diagram;
        private MapTerrainDetector[] _terrainDetectors;

        public IDebugLogger Logger { private get; set; } = null;

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
            _diagram = new(_map.Properties.Shape, _map.Properties.SizeMultiple);

            yield return _master.StartCoroutine(GenerateBuildingsOnMap(_diagram));
        
            yield return _master.StartCoroutine(GenerateDetectorsOnMap(_diagram));

            yield return _master.StartCoroutine(GenerateStuffByTerrain(_terrainDetectors));

            Logger.Add("generate is finished.");
        }
        private IEnumerator GenerateBuildingsOnMap(Matrix<MapNodeData> map)
        {
            #region generate base diagram

            var baseDiagGenerator = new MapBldgBaseDiagramGenerator(_map.Properties, _map.GameObjectDef, _map.Config.Tick);

            Logger?.Add               ("generating base diagram..");
            Logger?.AddDynamicPerc    (baseDiagGenerator.FinishedPercent, until: baseDiagGenerator.Completed);

            _master.StartCoroutine    (baseDiagGenerator.GenerateOnDiagram(map));
            yield return new WaitUntil(baseDiagGenerator.Completed);

            #endregion

            #region bake structures

            var structureGenerator = new MapBldgStructureDiagramGenerator(_map.GameObjectDef, _map.Config.Tick);

            Logger?.Add               ("baking structures..");
            Logger?.AddDynamicPerc    (structureGenerator.FinishedPercent, until: structureGenerator.Completed);

            _master.StartCoroutine    (structureGenerator.GenerateOnDiagram(map));
            yield return new WaitUntil(structureGenerator.Completed);

            if (_map.Config.PrintMapGridDiagram)
                map.PrintDebugGraph();

            if (_map.Config.ShowStructureGenerateResult)
                structureGenerator.PrintGenerateResult();

            #endregion

            #region generate building gameobjects

            var entityGenerator = new MapBldgEntityGenerator(_map.Properties, _map.GameObjectDef, _parent, _map.Config.Tick);

            Logger?.Add               ("generating buildings..");
            Logger?.AddDynamicPerc    (entityGenerator.FinishedPercent, until: entityGenerator.Completed);

            _master.StartCoroutine    (entityGenerator.GenerateByDiagram(map));
            yield return new WaitUntil(entityGenerator.Completed);

            #endregion
        }
        private IEnumerator GenerateDetectorsOnMap(Matrix<MapNodeData> map)
        {
            #region generate coords

            var coordsGenerator = new MapTileCoordsGenerator(_map.Properties, maxTick: _map.Config.Tick);

            Logger?.Add               ("generating coords..");
            Logger?.AddDynamicPerc    (coordsGenerator.FinishedPercent, until: coordsGenerator.Completed);

            _master.StartCoroutine    (coordsGenerator.GenerateCoords(map));
            yield return new WaitUntil(coordsGenerator.Completed);
            var coords =               coordsGenerator.Result.ToArray();

            #endregion

            #region generate detectors

            var detectorsGenerator = new MapTerrainDetectorGenerator(_map.Properties, _map.Config.TerrainDetector, _parent, maxTick: _map.Config.Tick);

            Logger?.Add               ("generating detectors..");
            Logger?.AddDynamicPerc    (detectorsGenerator.FinishedPercent, until: detectorsGenerator.Completed);

            _master.StartCoroutine    (detectorsGenerator.GenerateDetectors(coords));
            yield return new WaitUntil(detectorsGenerator.Completed);
            _terrainDetectors =        detectorsGenerator.Result.ToArray();

            #endregion
        }
        private IEnumerator GenerateStuffByTerrain(MapTerrainDetector[] terrain)
        {
            #region generate stuff distribution

            var distDiagGenerator = new MapStuffDistributionDiagramGenerator(_map.GameObjectDef, _map.Properties, maxTick: _map.Config.Tick);

            Logger?.Add               ("generating stuff dist..");
            Logger?.AddDynamicPerc    (distDiagGenerator.FinishedPercent, until: distDiagGenerator.Completed);

            _master.StartCoroutine    (distDiagGenerator.BakeDistribution(maxDensity: _terrainDetectors.Max(d => d.DensityValue)));
            yield return new WaitUntil(distDiagGenerator.Completed);
            var distribution =         distDiagGenerator.Result;

            if (_map.Config.ShowStuffDistributionInfo)
                distDiagGenerator.PrintDistributionDiagram();

            #endregion

            #region analyze stuff distribution

            var dataAnalyzer = new MapStuffDataAnalyzer(maxTick: _map.Config.Tick);

            Logger?.Add               ("analysing stuff dist..");
            Logger?.AddDynamicPerc    (dataAnalyzer.FinishedPercent, until: dataAnalyzer.Completed);

            _master.StartCoroutine    (dataAnalyzer.Analysis(terrain, distribution));
            yield return new WaitUntil(dataAnalyzer.Completed);
            var stuffObjData =         dataAnalyzer.Result;

            #endregion

            #region generate stuff gameobjects 

            var stuffEntityGenerator = new MapStuffEntityGenerator(_parent.StuffObjParent, maxTick: _map.Config.Tick);

            Logger?.Add               ("generating stuffs..");
            Logger?.AddDynamicPerc    (stuffEntityGenerator.FinishedPercent, until: stuffEntityGenerator.Completed);

            _master.StartCoroutine    (stuffEntityGenerator.GenerateStuffs(stuffObjData));
            yield return new WaitUntil(stuffEntityGenerator.Completed);

            #endregion

            yield return null;
        }
    }
}
