using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [SelectionBase]
    internal abstract class MapTileEntity : MonoBehaviour, IMapTileEntity
    {
        protected IMapHandler  _controller { get; private set; }
        protected TileProperty _properties { get; private set; }
        protected abstract Transform Parent { get; }

        /*
         *  implements
         */

        TileProperty IMapTileEntity.Property => _properties;
        GameObject IGameObject.gameObject => gameObject;

        void IMapTileEntity.Init(TileProperty properties, IMapHandler controller)
        {
            // init properties           
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _properties = properties;

            // init entity
            transform.SetPositionAndRotation(_properties.ActualPosition, _properties.Direction.ToRotation());
            transform.SetParent(Parent);
        }

    }
}