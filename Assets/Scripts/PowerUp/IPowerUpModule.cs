using System;
using Db.PowerUp;

namespace PowerUp
{
    public interface IPowerUpModule
    {
        Action<EPowerUp> OnPowerUpCollected { get; set; }
    }
}