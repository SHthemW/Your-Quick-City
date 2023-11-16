using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Instances.Player
{
    internal sealed class PlayerDataManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerComponents _components;

        [SerializeField]
        private PlayerProperties _basicProps;

        private PlayerStatistics _statistics = new();

        /*
         *  components getters
         */

        internal PlayerComponents Components => _components;
        internal PlayerProperties BasicProps => _basicProps;

        /*
         *  data servers
         */

        private PlayerMovementDataServer _movementDataServer;
        internal PlayerMovementDataServer MovementDataServer
        {
            get
            {
                if (_movementDataServer is null)
                    _movementDataServer = new(_basicProps);
                return _movementDataServer;
            }
        }

        private PlayerTowardDataServer _towardDataServer;
        internal PlayerTowardDataServer TowardDataServer
        {
            get
            {
                if (_towardDataServer is null)
                    _towardDataServer = new(_components);
                return _towardDataServer;
            }
        }

    }
}
