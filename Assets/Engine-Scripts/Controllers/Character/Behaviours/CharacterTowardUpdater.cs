using Game.General.Interfaces;
using System;
using UnityEngine;

namespace Game.Ctrller.Character
{
    public sealed class CharacterTowardUpdater
    {
        // stats
        private readonly ITowardDataServer _dataServer;

        // requirement components
        private readonly Transform _transform;   
        
        // static properties
        private readonly static Quaternion ROTATE_HALF_VALUE = Quaternion.Euler(0, 90, 0);      
        private const float TOWARD_JUDGE_X = 0.5f;
        private const float ROTATE_SPEED = 2f;

        // runtime properties     
        /// <summary>
        /// current target toward direction.<br/>
        /// </summary>
        /// <remarks>
        /// <see langword="true"/> means positive direction (localScale.x > 0)<br/>
        /// <see langword="false"/> means negative.
        /// </remarks>
        private bool _targetToward { get; set; } = true;
        
        public CharacterTowardUpdater(Transform transform, ITowardDataServer dataServer)
        {
            _transform = transform != null ? transform : throw new ArgumentNullException(nameof(transform));
            _dataServer = dataServer ?? throw new ArgumentNullException(nameof(dataServer));
        }
        public void UpdateTowardRotation()
        {
            UpdateCurrentTargetToward();

            var target = GetCurrentTargetRotation();

            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, target, ROTATE_SPEED);

            if (_transform.rotation.eulerAngles.y == 90)
            {
                SwitchCharacterLocalScale();
            }
        }

        private CharacterTowardUpdater() { }
        private Quaternion GetCurrentTargetRotation()
        {
            if (_targetToward == _dataServer.CharacterToward)
                return Quaternion.identity;
            else
                return ROTATE_HALF_VALUE;
        }
        private void UpdateCurrentTargetToward()
        {
            if (_dataServer.SpeedInAxisX > TOWARD_JUDGE_X)
                _targetToward = true;

            else if (_dataServer.SpeedInAxisX < -TOWARD_JUDGE_X)
                _targetToward = false;
        }
        private void SwitchCharacterLocalScale()
        {
            _transform.localScale = new Vector3(
                _transform.localScale.x * -1,
                _transform.localScale.y,
                _transform.localScale.z);
        }
    }
}
