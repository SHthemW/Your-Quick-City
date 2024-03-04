using UnityEngine;

namespace Yours.QuickCity.Shape
{
    [CreateAssetMenu(fileName = "New RectangleShape", menuName = "Map/Shape/Rectangle")]
    public sealed class RectangleShape_SO : Shape_SO
    {
        [field: SerializeField]
        public RectangleShape RectangleShape { get; set; }

        public override sealed IShape Shape => RectangleShape;
    }
}
