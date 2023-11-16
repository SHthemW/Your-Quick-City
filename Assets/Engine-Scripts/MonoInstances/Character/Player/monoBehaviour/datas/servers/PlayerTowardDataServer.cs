using Game.General.Interfaces;
using System;
using System.Collections.Generic;

namespace Game.Instances.Player
{
    internal sealed class PlayerTowardDataServer : ITowardDataServer
    {
        private readonly PlayerComponents _components;
        internal PlayerTowardDataServer(PlayerComponents components)
        {
            _components = components;
        }

        /*
         *  service
         */

        bool ITowardDataServer.CharacterToward => _components.Root.localScale.x > 0;
        float ITowardDataServer.SpeedInAxisX => _components.Rigidbody.velocity.x;
    }
}
