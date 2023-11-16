using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ctrller.Character
{
    [Serializable]
    public struct CharMoveProperty
    {
        [SerializeField] float _baseMoveSpeed;
        public float BaseMoveSpeed => _baseMoveSpeed;
    }

    [Serializable]
    public struct CharAnimProperty
    {
        [SerializeField] string _name_moveSpeed;
        public string NAME_MOVESPEED => _name_moveSpeed;
    }

    [CreateAssetMenu(fileName = "New Character", menuName = "Data/Character/Property")]
    public sealed class CharacterProperty_SO : ScriptableObject
    {
        [SerializeField]
        private CharMoveProperty _moveProperty;
        public CharMoveProperty MoveProperty => _moveProperty;

        [Space]

        [SerializeField]
        private CharAnimProperty _animProperty;
        public CharAnimProperty AnimProperty => _animProperty;
    }

}
