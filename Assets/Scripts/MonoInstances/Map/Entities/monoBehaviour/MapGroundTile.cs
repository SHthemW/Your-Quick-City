using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapGroundTile : MapTileEntity
    {
        protected override Transform Parent => _controller.GetFloorObjParent();
    }
}