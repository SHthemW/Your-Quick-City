using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    [CreateAssetMenu(fileName = "New Structure", menuName = "Map/Structure/New")]
    internal sealed class StructureData_SO : ScriptableObject, IStructure
    {
        [Header("Basic")]

        [SerializeField]
        private List<MatrixNode<MapNodeData>> _diagram;

        [SerializeField]
        [Tooltip("障碍物内部封闭结点的数量. 请务必正确填写.")]
        private int _closedNodeNum;

        [Header("Properties")]

        [SerializeField]
        [Tooltip("总生成数量.")]
        private int _generateNumber;

        [SerializeField]
        [Tooltip(
            "生成该结构的优先级. \n" +
            "Normal - 常规 \n" +
            "ReplaceExists - 生成时允许替换已有的建筑物 \n" +
            "Force - 强制生成, 不执行其它检查. 该选项保证地图上一定会出现指定数量的该结构")]
        private StructureGeneratePriority _generatePriority; 

        /*
         *  functions
         */

        private void OnValidate()
        {
            CheckDiagramValidity();
        }
        private void CheckDiagramValidity()
        {
            // check repeat:
            var res = _diagram.GroupBy(d => d.Coordinate)
                .Where(g => g.Count() > 1)
                .Select(r => r.Key)
                .ToList();

            if (res.Count > 0)
                Debug.LogWarning($"[Map Structure] 警告: 结构 {name} 的结点配置不合法, 坐标 {res[0]} 存在重复.");
        }

        /*
         *  implements
         */

        List<MatrixNode<MapNodeData>> IStructure.StructureDiagram => _diagram;
        int IStructure.ClosedNodeNum => _closedNodeNum;
        int IStructure.GenerateNumber => _generateNumber;
        StructureGeneratePriority IStructure.GeneratePriority => _generatePriority;
        string IStructure.Name => name;

    }
}
