using System;
using UnityEngine;

namespace Game.Ctrller.Character
{
    [CreateAssetMenu(fileName = "New Skin", menuName = "Data/Character/Skin")]
    public sealed class CharacterSkin_SO : ScriptableObject
    {
        [SerializeField] 
        private GameObject[] _skins;
        public GameObject[] Skins => _skins ?? throw new ArgumentNullException(nameof(_skins));
    }
}