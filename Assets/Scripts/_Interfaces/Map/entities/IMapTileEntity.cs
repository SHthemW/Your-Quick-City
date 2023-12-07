using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [Serializable]
    public struct Coord
    {
        [field: SerializeField]
        public int x { get; private set; }

        [field: SerializeField]
        public int y { get; private set; }

        public Coord(int x, int y)
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

    public readonly struct TileProperty
    {
        public Vector3 ActualPosition { get; }
        public Direction Direction { get; }

        public TileProperty(Vector3 actualCoord, Direction direction)
        {
            ActualPosition = actualCoord;
            Direction = direction;
        }
    }
}

namespace Yours.QuickCity.Internal
{
    public interface IMapTileEntity : IGameObject
    {
        TileProperty Property { get; }

        void Init(TileProperty properties, IMapHandler controller);
    }
}