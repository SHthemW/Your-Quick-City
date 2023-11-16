using Game.General.Interfaces;
using Game.General.Properties;
using System;
using UnityEngine;

namespace Game.General.Properties
{
    [Serializable]
    public sealed class MapDiagramNode : INodeDataHandler
    {
        [SerializeField]
        [Tooltip("该节点相对于原点(左下角)的坐标.")]
        private Coord _coordinate;

        [SerializeField]
        private MapDiagramNodeData _data;

        // properties
        public Coord Coordinate 
        { 
            get => _coordinate; 
            set => _coordinate = value; 
        }      
        public MapDiagramNodeData NodeData => _data;
        public bool IsObstacle => _data.NodeObj != null;

        // runtime setters
        public void PlaceObstacle(GameObject obstacle)
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
    public struct MapDiagramNodeData
    {
        [field: SerializeField]
        public Direction Direction { get; private set; }

        [field: SerializeField]
        [Tooltip("该节点应该生成的Object.")]
        public GameObject NodeObj { get; private set; }

        public MapDiagramNodeData(Direction direction, GameObject nodeObj)
        {
            Direction = direction;
            NodeObj = nodeObj;
        }
    }
    public enum Direction { Random = 0, Up, Down, Left, Right }
}

namespace Game.General.Interfaces
{
    /// <summary>
    /// data setter interface of <see cref="MapDiagramNode"/>
    /// </summary>
    public interface INodeDataHandler
    {
        MapDiagramNodeData Data { get; set; }
    }
}