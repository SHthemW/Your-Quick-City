
using UnityEngine;

namespace Game.General.Interfaces
{
    public interface IMapHandler
    {
        Transform GetFloorObjParent();
        Transform GetObstacleObjParent();
        Transform GetStuffObjParent();
        Transform GetStuffDetectorParent();
    }
}
