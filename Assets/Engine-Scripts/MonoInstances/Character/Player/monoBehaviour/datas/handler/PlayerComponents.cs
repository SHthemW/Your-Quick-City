using System;
using UnityEngine;
using static Game.Instances.Player.DataUtils;

namespace Game.Instances.Player
{
    [Serializable]
    internal struct PlayerComponents
    {     
        #region Behaviour

        [Header("Behaviour")]

        [SerializeField] Animator _characterAnimator;
        internal Animator CharacterAnimator => CheckAndGet(_characterAnimator);

        [SerializeField] Rigidbody _rigidbody;
        internal Rigidbody Rigidbody => CheckAndGet(_rigidbody);
        
        #endregion

        #region Structure

        [Header("Structure")]

        [SerializeField] Transform _root;
        internal Transform Root => CheckAndGet(_root);

        [SerializeField] Transform _skinTempParent;
        internal Transform SkinTempParent => CheckAndGet(_skinTempParent);

        [SerializeField] GameObject _modelObject;
        internal GameObject ModelObject => CheckAndGet(_modelObject);

        #endregion     
    }
}
