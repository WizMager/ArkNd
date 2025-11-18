using Db.PowerUp;
using Views;

namespace PowerUp
{
    public readonly struct ActivePowerUp
    {
        public readonly PowerUpView View;
        public readonly EPowerUp Type;

        public ActivePowerUp(PowerUpView view, EPowerUp type)
        {
            View = view;
            Type = type;
        }
    }
}