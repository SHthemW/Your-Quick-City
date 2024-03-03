using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [Serializable]
    internal sealed class MartrixNode<TData> where TData : IMatrixNodeData, new()
    {
        [SerializeField]
        [Tooltip("该节点相对于原点(左下角)的坐标.")]
        private Coord _coordinate;

        [SerializeField]
        private TData _data;

        internal Coord Coordinate 
        { 
            get => _coordinate; 
            set => _coordinate = value; 
        }      
        internal TData Data
        {
            get => _data; 
            set => _data = value;
        }

        internal MartrixNode()
        {
            this._data = new();
        }
    }

    internal interface IMatrixNodeData
    {
        bool HasContent { get; }
    }

    [Serializable]
    internal sealed class MapDiagramNodeData : IMatrixNodeData
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