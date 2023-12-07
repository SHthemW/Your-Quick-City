using System.Collections.Generic;
using UnityEngine;
using System;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapStuffEntityGenerator
    {
        private readonly Transform _generateParent;

        internal MapStuffEntityGenerator(Transform generateParent)
        {
            _generateParent = generateParent != null ? generateParent : throw new ArgumentNullException(nameof(generateParent));
        }

        internal void GenerateStuffs(Dictionary<(Vector3 pos, Vector3 attachDir), IStuff> stuffInfo)
        {
            foreach (var info in stuffInfo)
            {
                if (_generateCount.ContainsKey(info.Value) && 
                    _generateCount[info.Value] > info.Value.MaxGenerateNum)
                    continue;

                UnityEngine.Object.Instantiate(   
                    parent:   _generateParent,
                    original: info.Value.Obj,
                    position: new Vector3(info.Key.pos.x, 0, info.Key.pos.z),
                    rotation: info.Value.GetGenerateDirection
                    (
                        attachDirection: info.Key.attachDir, 
                        origRotation:    info.Value.Obj.transform.rotation.eulerAngles)
                    );

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
