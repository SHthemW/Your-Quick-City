using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [Serializable]
    internal sealed class MapNodeData : IMatrixNodeData
    {
        [field: SerializeField]
        internal Direction Direction { get; set; }

        [field: SerializeField]
        [Tooltip("该节点应该生成的Object.")]
        internal GameObject NodeObj { get; set; }

        public bool HasContent => NodeObj != null;
    }
    internal enum Direction { Random = 0, Up, Down, Left, Right }
}