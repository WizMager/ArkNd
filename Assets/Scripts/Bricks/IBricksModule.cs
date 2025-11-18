using System;
using UnityEngine;

namespace Bricks
{
    public interface IBricksModule
    {
        Action<Vector2> OnPowerUp { get; set; }
    }
}