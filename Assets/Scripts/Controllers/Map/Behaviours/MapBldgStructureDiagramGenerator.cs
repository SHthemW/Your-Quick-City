using Game.General.Interfaces;
using Game.General.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ctrller.Map
{
    public sealed class MapBldgStructureDiagramGenerator
    {
        private readonly MapStructureGenerationProperty _properties;

        private Dictionary<Coord, MapDiagramNodeData> _finalStructureDiagram { get; set; } = new();

        /*
         *  public:
         */

        public MapBldgStructureDiagramGenerator(MapStructureGenerationProperty properties)
        {
            _properties = properties;
        }
        public void GenerateOnDiagram(MapDiagram diagram)
        {
            foreach (var structure in _properties.StructureList)
            {
                TryAddStructuresToDiagram(structure, diagram);
            }
        }

        /*
         *  private:
         */

        private MapBldgStructureDiagramGenerator() { }
        private void TryAddStructuresToDiagram(IStructure structure, MapDiagram diagram)
        {
            HashSet<Coord> succeedCoords = new();
            HashSet<Coord> failureCoords = new();

            for (int times = 0, success = 0; times < diagram.TotalNodeNum && success < structure.GenerateNumber; times++)
            {              
                var tryingCoord = ChooseRandomCoord();

                if (!JudgeIfCanGenerateStructure(structure, diagram, tryingCoord))
                {
                    failureCoords.Add(tryingCoord);
                    continue;
                }

                if (_properties.EnableStructureDebug)
                    Debug.Log($"[structure] 结构 {structure.Name} 的第 {times + 1} 次尝试生成成功, 生成位置: {tryingCoord}");

                success++;
                succeedCoords.Add(tryingCoord);

                WriteStructureToDiagram(structure, diagram);
            }

            if (_properties.EnableStructureDebug && (succeedCoords.Count < structure.GenerateNumber))
                Debug.Log($"[structure] 结构 {structure.Name} 未完成其生成目标 ({succeedCoords.Count} / {structure.GenerateNumber}), 因为整个地图中没有合法的位置供其生成. ");

            // local function
            Coord ChooseRandomCoord()
            {
                var result = GetRandomCoord();

                while (succeedCoords.Contains(result) || failureCoords.Contains(result))
                    result = GetRandomCoord();

                return result;

                Coord GetRandomCoord()
                {
                    int random_x = UnityEngine.Random.Range(0, diagram.SizeX);
                    int random_y = UnityEngine.Random.Range(0, diagram.SizeY);
                    return new Coord(random_x, random_y);
                }
            }
            
        }
        /// <summary>
        /// judge if given coordinate can generate such structure <br/>
        /// that storaged in <see langword="this"/>, without any accessible problem.<br/>
        /// </summary>
        /// <param name="diagram"></param>
        /// <param name="tryPoint"></param>
        /// <returns></returns>
        private bool JudgeIfCanGenerateStructure(IStructure structure, MapDiagram diagram, Coord tryPoint)
        {
            foreach (var node in structure.StructureDiagram)
            {
                var mapped_coord = tryPoint + node.Coordinate;
               
                if (!diagram.JudgeIfCanPlaceObstacle(mapped_coord) && !CanGenerateForcibly(mapped_coord))
                {
                    _finalStructureDiagram.Clear();
                    return false;
                }
                _finalStructureDiagram.Add(mapped_coord, (node as INodeDataHandler).Data);
            }
            return true;        

            bool CanGenerateForcibly(Coord coord)
            {                
                if (diagram.JudgeCoordIfOutOfRange(coord))
                    return false;

                else if(structure.GeneratePriority == StructureGeneratePriority.Force)
                    return true;

                else if (structure.GeneratePriority == StructureGeneratePriority.ReplaceExists)
                    return diagram[coord.x, coord.y].IsObstacle;

                else 
                    return false;
            }
        }
        private void WriteStructureToDiagram(IStructure structure, MapDiagram diagram)
        {
            if (_finalStructureDiagram.Count == 0)
                throw new InvalidOperationException("[Map]: 无法生成结构, 因为没有可供生成的final diagram.");

            foreach (var nodeKvp in _finalStructureDiagram)
            {
                (diagram[nodeKvp.Key.x, nodeKvp.Key.y] as INodeDataHandler).Data = nodeKvp.Value;
            }
            diagram.ClosedNodeNum += structure.ClosedNodeNum;

            _finalStructureDiagram.Clear();
        }
    }
}
