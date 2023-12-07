using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [SelectionBase]
    internal abstract class MapTileEntity : MonoBehaviour, IMapTileEntity
    {
        private protected IMapObjParent Controller { get; private set; }
        private protected TileProperty Properties { get; private set; }
        private protected abstract Transform Parent { get; }

        /*
         *  implements
         */

        TileProperty IMapTileEntity.Property => Properties;
        GameObject IGameObject.gameObject => gameObject;

        void IMapTileEntity.Init(TileProperty properties, IMapObjParent controller)
        {
            // init properties           
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            Properties = properties;

            // init entity
            transform.SetPositionAndRotation(Properties.ActualPosition, Properties.Direction.ToRotation());
            transform.SetParent(Parent);
        }

    }
}