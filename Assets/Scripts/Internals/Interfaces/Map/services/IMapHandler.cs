
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    public interface IMapHandler
    {
        Transform GetFloorObjParent();
        Transform GetObstacleObjParent();
        Transform GetStuffObjParent();
        Transform GetStuffDetectorParent();
    }
}
