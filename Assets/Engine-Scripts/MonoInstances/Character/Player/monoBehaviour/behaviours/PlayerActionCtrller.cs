using Game.Ctrller.Character;
using Game.General.Interfaces;
using UnityEngine;

namespace Game.Instances.Player
{
    internal sealed class PlayerActionCtrller : PlayerBehaviour
    {
        // physics
        private CharacterMover _moveCtrller;
        private CharacterTowardUpdater _towardCtrller;

        // view
        private CharacterAnimUpdater _animCtrller;
        private CharacterSkinReplacer _skinReplacer;
      
        /*
         *  Machine Behaviour
         */

        private void Start()
        {
            _moveCtrller = new(_components.Rigidbody, _data.MovementDataServer, GetComponent<IMovementInputer>());
            _towardCtrller = new(_components.Root, _data.TowardDataServer);

            _skinReplacer = new(_components.ModelObject, _components.SkinTempParent);
            _animCtrller = new(_components.CharacterAnimator, _properties.Character.AnimProperty);          
        }

        private void Update()
        {
            _moveCtrller.Move();
            _towardCtrller.UpdateTowardRotation();

            GetInputAndReplaceSkinTest();
            _animCtrller.UpdateMoveAnim(_components.Rigidbody.velocity.sqrMagnitude);           
        }

        /*
         *  Test
         */

        private void GetInputAndReplaceSkinTest()
        {
            if (Input.GetKeyDown(KeyCode.R))
                _skinReplacer.SwitchSkin(_properties.TestSkin.Skins);
        }
    }
}


