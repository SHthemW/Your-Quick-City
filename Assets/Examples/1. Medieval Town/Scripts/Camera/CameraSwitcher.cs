using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Yours.QuickCity.Internal
{
    public sealed class CameraSwitcher : MonoBehaviour
    {
        [SerializeField]
        private Camera _staticCam;

        [SerializeField]
        private Camera _dynamicCam;

        public void SwitchToDynamicCamera(bool val)
        {
            _staticCam.gameObject.SetActive(!val);
            _dynamicCam.gameObject.SetActive(val);
        }
    }
}