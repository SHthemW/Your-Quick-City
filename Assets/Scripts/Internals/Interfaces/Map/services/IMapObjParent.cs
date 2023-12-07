
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    public interface IMapObjParent
    {
        Transform GetFloorObjParent();
        Transform GetObstacleObjParent();
        Transform GetStuffObjParent();
        Transform GetStuffDetectorParent();
    }
}
