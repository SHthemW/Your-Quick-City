using Yours.QuickCity.Internal;
using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal static class Extensions
    {
        internal static Direction RandomValue(this Direction direction)
        {
            if (direction != Direction.Random)
                return direction;

            int seed = UnityEngine.Random.Range((int)Direction.Random + 1, 4);

            return seed switch
            {
                1 => Direction.Up,
                2 => Direction.Down,
                3 => Direction.Left,
                4 => Direction.Right,

                _ => throw new NotImplementedException($"[Enum] 枚举 {nameof(Direction)} 的值 {seed} 未定义.")
            };
        }
        internal static Quaternion ToRotation(this Direction direction)
        {
            if (direction == Direction.Random)
                direction = direction.RandomValue();

            return direction switch
            {
                Direction.Up => Quaternion.Euler(0, 0, 0),
                Direction.Down => Quaternion.Euler(0, 180, 0),
                Direction.Left => Quaternion.Euler(0, 90, 0),
                Direction.Right => Quaternion.Euler(0, 270, 0),

                _ => throw new NotImplementedException($"[Enum] 枚举 {nameof(Direction)} 的转向 {direction} 未定义."),
            };
        }

        internal static bool IsNSWE(this Vector3 direction)
        {
            return direction == Vector3.forward || direction == Vector3.back
                   || direction == Vector3.left || direction == Vector3.right;
        }
    }
}
