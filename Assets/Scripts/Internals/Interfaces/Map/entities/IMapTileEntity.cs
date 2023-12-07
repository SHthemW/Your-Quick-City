using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [Serializable]
    internal struct Coord
    {
        [field: SerializeField]
        internal int x { get; private set; }

        [field: SerializeField]
        internal int y { get; private set; }

        internal Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Coord c1, Coord c2)
        {
            return (c1.x == c2.x) && (c1.y == c2.y);
        }
        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }
        public static Coord operator +(Coord c1, Coord c2)
        {
            return new Coord(c1.x + c2.x, c1.y + c2.y);
        }

        public override bool Equals(object obj)
        {
            return obj is Coord coord &&
                   x == coord.x &&
                   y == coord.y;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
        public override string ToString()
        {
            return $"x: {x}, y: {y}";
        }
    }

    internal readonly struct TileProperty
    {
        internal Vector3 ActualPosition { get; }
        internal Direction Direction { get; }

        internal TileProperty(Vector3 actualCoord, Direction direction)
        {
            ActualPosition = actualCoord;
            Direction = direction;
        }
    }
}

namespace Yours.QuickCity.Internal
{
    internal interface IMapTileEntity : IGameObject
    {
        TileProperty Property { get; }

        void Init(TileProperty properties, IMapHandler controller);
    }
}