using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapBldgStructureDiagramGenerator : StepwiseTask
    {
        private readonly MapEntities _mapObjects;

        private readonly StringBuilder _resultMessage = new("结构生成信息: \n");

        private Dictionary<Coord, MapNodeData> _finalStructureDiagram { get; set; } = new();

        /*
         *  internal:
         */

        internal MapBldgStructureDiagramGenerator(MapEntities objectsDef, int maxTick) : base(maxTick)
        {
            _mapObjects = objectsDef;
        }
        internal IEnumerator GenerateOnDiagram(Matrix<MapNodeData> diagram)
        {
            yield return Foreach(iter: _mapObjects.StructureList, stepCount: _mapObjects.StructureList.Count / 2, body: structure =>
            {
                TryAddStructuresToDiagram(structure, diagram);
            });
        }
        internal void PrintGenerateResult()
        {
            Debug.Log(_resultMessage.ToString());
        }

        /*
         *  private:
         */

        private MapBldgStructureDiagramGenerator() : base(-1)
            => throw new NotImplementedException();
        private void TryAddStructuresToDiagram(IStructure structure, Matrix<MapNodeData> diagram)
        {
            var totalCoords  = diagram.Content.Select(n => n.Coordinate).ToArray();
            int successCount = 0;
            
            for (int times = 0; times < totalCoords.Count() && successCount < structure.GenerateNumber; times++)
            {
                Queue<Coord> randoms = new(MapUtils.ShuffleRandomly(totalCoords));

                if (!randoms.TryDequeue(out Coord tryingCoord))
                    break;
               
                if (!JudgeIfCanGenerateStructure(structure, diagram, tryingCoord))
                    continue;

                _resultMessage.AppendLine($"[structure] 结构 {structure.Name} 的第 {times + 1} 次尝试生成成功, 生成位置: {tryingCoord}");

                successCount++;

                WriteStructureToDiagram(structure, diagram);
            }

            if (successCount < structure.GenerateNumber)
                _resultMessage.AppendLine($"[structure] 结构 {structure.Name} 未完成其生成目标 ({successCount} / {structure.GenerateNumber}), 因为整个地图中没有合法的位置供其生成. ");
        }
        /// <summary>
        /// judge if given coordinate can generate such structure <br/>
        /// that storaged in <see langword="this"/>, without any accessible problem.<br/>
        /// </summary>
        /// <param name="diagram"></param>
        /// <param name="tryPoint"></param>
        /// <returns></returns>
        private bool JudgeIfCanGenerateStructure(IStructure structure, Matrix<MapNodeData> diagram, Coord tryPoint)
        {
            foreach (var node in structure.StructureDiagram)
            {
                var mapped_coord = tryPoint + node.Coordinate;
               
                if (!diagram.StillConnectedWhenContented(mapped_coord) && !CanGenerateForcibly(mapped_coord))
                {
                    _finalStructureDiagram.Clear();
                    return false;
                }
                _finalStructureDiagram.Add(mapped_coord, node.Data);
            }
            return true;        

            bool CanGenerateForcibly(Coord coord)
            {                
                if (diagram.CoordIsOutOfBounds(coord))
                    return false;

                else if(structure.GeneratePriority == StructureGeneratePriority.Force)
                    return true;

                else if (structure.GeneratePriority == StructureGeneratePriority.ReplaceExists)
                    return diagram[coord.x, coord.y].Data.HasContent;

                else 
                    return false;
            }
        }
        private void WriteStructureToDiagram(IStructure structure, Matrix<MapNodeData> diagram)
        {
            if (_finalStructureDiagram.Count == 0)
                throw new InvalidOperationException("[Map]: 无法生成结构, 因为没有可供生成的final diagram.");

            foreach (var nodeKvp in _finalStructureDiagram)
            {
                diagram[nodeKvp.Key.x, nodeKvp.Key.y].Data = nodeKvp.Value;
            }
            diagram.ClosedNodeNum += structure.ClosedNodeNum;

            _finalStructureDiagram.Clear();
        }
    }
}
