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
        /// when enable, the structure will replace the original structure during generation.
        /// </summary>
        bool ForceGenerate { get; }

        string Name { get; }
    }
}
