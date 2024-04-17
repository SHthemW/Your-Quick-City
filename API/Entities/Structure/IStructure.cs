using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal interface IStructure
    {
        List<MatrixNode<MapNodeData>> StructureDiagram { get; }
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

    internal enum StructureGeneratePriority
    {
        Normal, ReplaceExists, Force
    }
}
