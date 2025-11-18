using UnityEngine;

namespace Db.Game
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [field:SerializeField] public float DefaultBallSpeed;
        [field:SerializeField] public float PlatformSpeed;
        [field:SerializeField] public int DestroyBlockCountPowerUp;
        [field:SerializeField] public float PowerUpFallSpeed = 2f;
        
        [Header("Platform Bounce Angles")]
        [field:SerializeField] public float MinBounceAngle = 0f;
        [field:SerializeField] public float MaxBounceAngle = 75f;
    }
}