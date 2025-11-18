using System.Collections.Generic;
using UnityEngine;

namespace Db.PowerUp
{
    [CreateAssetMenu(fileName = "PowerUpData", menuName = "Data/PowerUpData")]
    public class PowerUpData : ScriptableObject
    {
        [SerializeField] private List<PowerUpPair> _powerUps;

        [field: SerializeField] public float PlatformSizeChange { get; private set; } = 1f;

        public IReadOnlyList<PowerUpPair> PowerUps => _powerUps;

        public bool TryGetRandomPowerUp(out PowerUpPair powerUp)
        {
            powerUp = default;
            
            if (_powerUps == null || _powerUps.Count == 0)
            {
                return false;
            }

            var index = Random.Range(0, _powerUps.Count);
            powerUp = _powerUps[index];
            return true;
        }
    }
}