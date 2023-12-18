using UnityEngine;
using System.Collections.Generic;

namespace Yours.QuickCity.Internal
{
    internal static class MapUtils
    {
        /// <summary>
        /// shuffle input data randomly, then return it.
        /// </summary>
        internal static T[] ShuffleRandomly<T>(in T[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                int randomNum = UnityEngine.Random.Range(i, elements.Length);

                // swap
                (elements[i], elements[randomNum]) = (elements[randomNum], elements[i]);
            }
            return elements;
        }

        /// <summary>
        /// convert the logic coord to actual coord in world.
        /// </summary>
        /// <param name="unitSize">edge length of each map tile</param>
        /// <param name="logicCoord">logic coord to be converted</param>
        /// <returns>convert result</returns>
        internal static Vector3 GetTileActualPosition(float unitSize, Coord logicCoord)
        {
            return new Vector3(
                unitSize / 2 + logicCoord.x * unitSize,
                0,
                unitSize / 2 + logicCoord.y * unitSize
                );
        }

        /// <summary>
        /// get map debug color between RED and GREEN.
        /// </summary>
        /// <param name="percentVal">0(green) - 1(red).</param>
        /// <returns> color between red and green.<br/>
        /// others probably means invalid value. </returns>
        internal static Color GetDebugColor(float percentVal)
        {
            if (percentVal < 0)
                return Color.black;

            if (percentVal > 1)
                return Color.green;

            float green = Mathf.Min(1, percentVal * 2);
            float red = Mathf.Min(1, -(percentVal * 2) + 2f);

            return new Color(red, green, 0);
        }
    }
}
