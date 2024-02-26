using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace Yours.QuickCity.Internal
{
    internal sealed class MapStuffEntityGenerator : StepwiseTask
    {
        private readonly Transform _generateParent;

        internal MapStuffEntityGenerator(Transform generateParent, int maxTick) : base(maxTick)
        {
            _generateParent = generateParent != null ? generateParent : throw new ArgumentNullException(nameof(generateParent));
        }

        internal IEnumerator GenerateStuffs(Dictionary<(Vector3 pos, Vector3 attachDir), IStuff> stuffInfo)
        {
            yield return Foreach(iter: stuffInfo, stepCount: stuffInfo.Count, body: info => 
            {
                if (_generateCount.ContainsKey(info.Value) &&
                    _generateCount[info.Value] > info.Value.MaxGenerateNum)
                    throw new ContinueException();

                UnityEngine.Object.Instantiate(
                    parent: _generateParent,
                    original: info.Value.Obj,
                    position: new Vector3(info.Key.pos.x, 0, info.Key.pos.z),
                    rotation: info.Value.GetGenerateDirection
                    (
                        attachDirection: info.Key.attachDir,
                        origRotation: info.Value.Obj.transform.rotation.eulerAngles)
                    );

                if (_generateCount.ContainsKey(info.Value))
                    _generateCount[info.Value]++;
                else
                    _generateCount.Add(info.Value, 1);
            });
        }

        private MapStuffEntityGenerator() : base(-1)
            => throw new InvalidOperationException();

        private readonly Dictionary<IStuff, int> _generateCount = new();
    }
}
