namespace Yours.QuickCity.Internal
{
    internal interface IMapStuffEntity
    {
        bool TryInit(IStuff conf, MapTerrainDetector data);
        bool IsInited();
    }
}
