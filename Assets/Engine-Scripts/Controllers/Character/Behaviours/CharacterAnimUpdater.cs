using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ctrller.Character
{
    public class CharacterAnimUpdater
    {
        private readonly Animator _animator;
        private readonly CharAnimProperty _properties;


        public CharacterAnimUpdater(Animator animator, CharAnimProperty properties)
        {
            _animator = animator != null ? animator : throw new ArgumentNullException(nameof(animator));
            _properties = properties;
        }
        public void UpdateMoveAnim(float currentSpeed)
        {
            _animator.SetFloat(_properties.NAME_MOVESPEED, currentSpeed);
        }

        private CharacterAnimUpdater() { }
    }
}
