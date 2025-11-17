using UnityEngine;

namespace Db.Game
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        [field: SerializeField] public float StartBallSpeed;
        [field: SerializeField] public float PlatformSpeed;
    }
}