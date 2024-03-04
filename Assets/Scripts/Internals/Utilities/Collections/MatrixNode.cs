using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [Serializable]
    internal sealed class MatrixNode<TData> where TData : IMatrixNodeData, new()
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

        internal MatrixNode()
        {
            this._data = new();
        }
    }

    internal interface IMatrixNodeData
    {
        bool HasContent { get; }
    }
}