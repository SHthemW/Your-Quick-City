using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [Serializable]
    internal sealed class MapDiagramNode : INodeDataHandler
    {
        [SerializeField]
        [Tooltip("该节点相对于原点(左下角)的坐标.")]
        private Coord _coordinate;

        [SerializeField]
        private MapDiagramNodeData _data;

        // properties
        internal Coord Coordinate 
        { 
            get => _coordinate; 
            set => _coordinate = value; 
        }      
        internal MapDiagramNodeData NodeData => _data;
        internal bool IsObstacle => _data.NodeObj != null;

        // runtime setters
        internal void PlaceObstacle(GameObject obstacle)
        {
            var temp = _data;
            _data = new(temp.Direction, obstacle);
        }

        // implements
        MapDiagramNodeData INodeDataHandler.Data
        {
            get => _data;
            set => _data = value;
        }
    }

    [Serializable]
    internal struct MapDiagramNodeData
    {
        [field: SerializeField]
        internal Direction Direction { get; private set; }

        [field: SerializeField]
        [Tooltip("该节点应该生成的Object.")]
        internal GameObject NodeObj { get; private set; }

        internal MapDiagramNodeData(Direction direction, GameObject nodeObj)
        {
            Direction = direction;
            NodeObj = nodeObj;
        }
    }
    internal enum Direction { Random = 0, Up, Down, Left, Right }
}

namespace Yours.QuickCity.Internal
{
    /// <summary>
    /// data setter interface of <see cref="MapDiagramNode"/>
    /// </summary>
    internal interface INodeDataHandler
    {
        MapDiagramNodeData Data { get; set; }
    }
}