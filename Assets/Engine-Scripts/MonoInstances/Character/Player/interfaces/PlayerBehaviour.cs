using UnityEngine;

namespace Game.Instances.Player
{
    internal abstract class PlayerBehaviour : MonoBehaviour
    {
        protected PlayerDataManager _data { get; private set; }
        protected PlayerComponents _components => _data.Components;
        protected PlayerProperties _properties => _data.BasicProps;

        private void Awake()
        {
            _data = GetComponent<PlayerDataManager>();
        }
    }
}
