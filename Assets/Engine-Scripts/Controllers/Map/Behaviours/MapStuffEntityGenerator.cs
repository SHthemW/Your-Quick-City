using Game.General.Interfaces;
using Game.General.Properties;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace Game.Ctrller.Map
{
    public sealed class MapStuffEntityGenerator
    {
        private readonly Transform _generateParent;

        public MapStuffEntityGenerator(Transform generateParent)
        {
            _generateParent = generateParent != null ? generateParent : throw new ArgumentNullException(nameof(generateParent));
        }

        public void GenerateStuffs(Dictionary<Vector3, IStuff> stuffInfo)
        {
            foreach (var info in stuffInfo)
            {
                if (_generateCount.ContainsKey(info.Value) && 
                    _generateCount[info.Value] > info.Value.MaxGenerateNum)
                    continue;

                UnityEngine.Object.Instantiate(   
                    parent:   _generateParent,
                    original: info.Value.Obj,
                    position: new Vector3(info.Key.x, 0, info.Key.z),
                    rotation: info.Value.Obj.transform.rotation);

                if (_generateCount.ContainsKey(info.Value))
                    _generateCount[info.Value]++;
                else
                    _generateCount.Add(info.Value, 1);
            }
        }

        private MapStuffEntityGenerator()
            => throw new NotImplementedException();

        private readonly Dictionary<IStuff, int> _generateCount = new();
    }
}
