
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal interface IMapHandler
    {
        Transform GetFloorObjParent();
        Transform GetObstacleObjParent();
        Transform GetStuffObjParent();
        Transform GetStuffDetectorParent();
    }
}
