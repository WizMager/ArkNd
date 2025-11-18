using System;
using UnityEngine;

namespace Db.PowerUp
{
    [Serializable]
    public struct PowerUpPair
    {
        public EPowerUp PowerUp;
        public Color BackgroundColor;
        public Color IconColor;
    }
}