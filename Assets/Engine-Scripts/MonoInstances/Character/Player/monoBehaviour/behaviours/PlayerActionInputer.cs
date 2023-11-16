using Game.General.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Instances.Player
{
    internal sealed class PlayerActionInputer : MonoBehaviour, IMovementInputer
    {
        Vector2 IMovementInputer.GetInputDirection()
        {
            var input_x = Input.GetAxisRaw("Horizontal");
            var input_y = Input.GetAxisRaw("Vertical");

            return new Vector2(input_x, input_y);
        }
    }
}
