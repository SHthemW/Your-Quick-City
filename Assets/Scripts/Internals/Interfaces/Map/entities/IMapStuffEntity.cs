namespace Yours.QuickCity.Internal
{
    internal interface IMapStuffEntity
    {
        bool TryInit(IStuff conf, IMapTerrainDetector data);
        bool IsInited();
    }
}
