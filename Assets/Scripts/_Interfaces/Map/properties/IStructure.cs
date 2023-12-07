using Game.General.Properties;
using System.Collections.Generic;

namespace Game.General.Interfaces
{
    public interface IStructure
    {
        List<MapDiagramNode> StructureDiagram { get; }
        /// <summary>
        /// number of node that closed in structure internal, <br/>
        /// which not connected to the outside map.
        /// </summary>
        int ClosedNodeNum { get; }
        /// <summary>
        /// number of this structure generated. bufore achieve this target, program will <br/>
        /// generate continuously.
        /// </summary>
        int GenerateNumber { get; }
        /// <summary>
        /// priority of structure generation. higher value means more mandatory generate.
        /// </summary>
        StructureGeneratePriority GeneratePriority { get; }

        string Name { get; }
    }

    public enum StructureGeneratePriority
    {
        Normal, ReplaceExists, Force
    }
}
