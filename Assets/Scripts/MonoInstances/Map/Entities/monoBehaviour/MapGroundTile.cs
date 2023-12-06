using Game.General.Interfaces;
using Game.General.Properties;
using UnityEngine;

namespace Game.Instances.Map.Entities
{
    internal sealed class MapGroundTile : MapTileEntity
    {
        protected override Transform Parent => _controller.GetFloorObjParent();
    }
}