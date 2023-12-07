using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapGroundTile : MapTileEntity
    {
        private protected override sealed Transform Parent => Controller.GetFloorObjParent();
    }
}