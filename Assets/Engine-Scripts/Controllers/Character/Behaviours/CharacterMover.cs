using Game.General.Interfaces;
using System;
using UnityEngine;

namespace Game.Ctrller.Character
{
    public sealed class CharacterMover
    {
        // components
        private readonly Rigidbody _rigidbody;

        // services
        private readonly IMovementInputer _inputer;
        private readonly IMovementDataServer _dataServer;
 
        // properties
        private const float DIAGONAL_SPEED_COMP = 0.7f;
        private Vector2 _currentDirection => _inputer.GetInputDirection();

        /*
         *  public:
         */

        public CharacterMover(Rigidbody rigidbody, IMovementDataServer dataServer, IMovementInputer inputer)
        {
            _rigidbody = rigidbody != null ? rigidbody : throw new ArgumentNullException(nameof(rigidbody));
            _inputer = inputer ?? throw new ArgumentNullException(nameof(inputer));
            _dataServer = dataServer ?? throw new ArgumentNullException(nameof(dataServer));
        }
        public void Move()
        {
            var targetForce = _currentDirection * _dataServer.GetMoveSpeed();

            if (_currentDirection.x != 0 && _currentDirection.y != 0)
                targetForce *= DIAGONAL_SPEED_COMP;

            _rigidbody.AddForce(new Vector3(targetForce.x, 0, targetForce.y));
        }

        /*
         *  private:
         */

        private CharacterMover() { }

    }

}


