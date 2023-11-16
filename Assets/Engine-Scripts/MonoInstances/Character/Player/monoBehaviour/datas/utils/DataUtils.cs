using System;

namespace Game.Instances.Player
{
    internal static class DataUtils
    {
        internal static T CheckAndGet<T>(T obj) where T : class
        {
            // notice:
            // do not use COALESCE expression, bucause UnityEngine.Object CANNOT
            // compare equality in default way.
            return obj != null ? obj : throw new ArgumentNullException(nameof(obj));
        }
    }
}
