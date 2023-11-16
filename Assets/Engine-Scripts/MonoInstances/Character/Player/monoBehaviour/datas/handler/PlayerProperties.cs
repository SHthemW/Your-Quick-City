using Game.Ctrller.Character;
using UnityEngine;
using System;
using static Game.Instances.Player.DataUtils;

namespace Game.Instances.Player
{
    [Serializable]
    internal struct PlayerProperties
    {       
        [SerializeField] CharacterSkin_SO _testSkin;
        internal CharacterSkin_SO TestSkin => CheckAndGet(_testSkin);

        [SerializeField] CharacterProperty_SO _character;
        internal CharacterProperty_SO Character => CheckAndGet(_character);
    }
}
