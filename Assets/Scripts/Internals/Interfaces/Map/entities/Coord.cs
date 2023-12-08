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

        public override readonly bool Equals(object obj)
        {
            return obj is Coord coord &&
                   x == coord.x &&
                   y == coord.y;
        }
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
        public override readonly string ToString()
        {
            return $"x: {x}, y: {y}";
        }
    }
}