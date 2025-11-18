using Db.PowerUp;
using UnityEngine;

namespace Views
{
    public class PowerUpView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private SpriteRenderer _icon;

        public void Initialize(PowerUpPair powerUpPair)
        {
            _background.color = powerUpPair.BackgroundColor;
            _icon.color = powerUpPair.IconColor;
        }
    }
}