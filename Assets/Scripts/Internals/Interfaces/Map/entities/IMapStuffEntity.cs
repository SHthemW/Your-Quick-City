namespace Yours.QuickCity.Internal
{
    public interface IMapStuffEntity
    {
        bool TryInit(IStuff conf, IMapTerrainDetector data);
        bool IsInited();
    }
}
