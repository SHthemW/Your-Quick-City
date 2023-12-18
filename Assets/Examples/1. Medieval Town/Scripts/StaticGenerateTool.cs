using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal sealed class StaticGenerateTool : MonoBehaviour
    {
        [SerializeField]
        private GameObject _toBeGenerate;

        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private Vector2Int _size;

        [SerializeField]
        private Transform _startPoint;

        [SerializeField]
        private float _mergeOffset;

        [SerializeField]
        private float _height;

        [ContextMenu("Generate")]
        private void GenerateQuads()
        {
            for (float i = 0; i < _size.x; i += _toBeGenerate.transform.localScale.x - _mergeOffset)
            {
                for (float j = 0; j < _size.y; j += _toBeGenerate.transform.localScale.y - _mergeOffset)
                {
                    GameObject quad = Instantiate(_toBeGenerate);
                    quad.transform.position = new Vector3(i, _height, j) + _startPoint.position;
                    quad.transform.parent = _parent;
                }
            }
        }

        [ContextMenu("Merge")]
        private void MergeQuads()
        {
            MeshFilter[] meshFilters = _parent.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            const string mergedObj   = "merged";
            const string unmergedObj = "unmerged";

            var mergedObject = _parent.Find(mergedObj) == null
                ? new GameObject(mergedObj)
                : _parent.Find(mergedObj).gameObject;
            mergedObject.transform.parent = _parent;

            var unmergedObjects = _parent.Find(unmergedObj) == null
                ? new GameObject(unmergedObj)
                : _parent.Find(unmergedObj).gameObject;
            unmergedObjects.transform.parent = _parent;

            for (int i = 0; i < meshFilters.Length; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

                meshFilters[i].gameObject.SetActive(false);
                meshFilters[i].transform.parent = unmergedObjects.transform;
            }

            mergedObject.AddComponent<MeshFilter>().mesh = new Mesh();
            mergedObject.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
            mergedObject.AddComponent<MeshRenderer>().sharedMaterial = _toBeGenerate.GetComponent<MeshRenderer>().sharedMaterial;

            mergedObject.SetActive(true);
        }

        [ContextMenu("Clean")]
        private void CleanQuads()
        {
            while (_parent.childCount > 0)
                DestroyImmediate(_parent.GetChild(0).gameObject);
        }
    }
}


