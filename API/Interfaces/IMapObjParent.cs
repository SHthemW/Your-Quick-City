
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    public interface IMapObjParent
    {
        Transform FloorObjParent { get; }

        Transform ObstacleObjParent { get; }

        Transform StuffObjParent { get; }
        Transform TerrainDetectorParent { get; }
    }
}
