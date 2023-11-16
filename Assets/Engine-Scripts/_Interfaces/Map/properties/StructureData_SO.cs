using Game.General.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.General.Properties
{
    [CreateAssetMenu(fileName = "New Structure", menuName = "Data/MapStructure")]
    public sealed class StructureData_SO : ScriptableObject, IStructure
    {
        [Header("Basic")]

        [SerializeField]
        private List<MapDiagramNode> _diagram;

        [SerializeField]
        [Tooltip("障碍物内部封闭结点的数量. 请务必正确填写.")]
        private int _closedNodeNum;

        [Header("Properties")]

        [SerializeField]
        [Tooltip("总生成数量.")]
        private int _generateNumber;

        [SerializeField]
        [Tooltip("若启用, 生成该结构时会尝试替换原有的障碍物.")]
        private bool _forceGenerate;

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

        List<MapDiagramNode> IStructure.StructureDiagram => _diagram;
        int IStructure.ClosedNodeNum => _closedNodeNum;
        int IStructure.GenerateNumber => _generateNumber;
        bool IStructure.ForceGenerate => _forceGenerate;
        string IStructure.Name => name;
    }
}
