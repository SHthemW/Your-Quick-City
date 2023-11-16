using Game.General.Interfaces;
using System;
using System.Collections.Generic;

namespace Game.Instances.Player
{
    internal sealed class PlayerMovementDataServer : IMovementDataServer
    {
        private readonly PlayerProperties _properties;
        internal PlayerMovementDataServer(PlayerProperties properties)
        {
            _properties = properties;
        }

        /*
         *  service
         */

        float IMovementDataServer.GetMoveSpeed()
        {
            return _properties.Character.MoveProperty.BaseMoveSpeed;
        }
    }
}
