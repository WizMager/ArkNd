using UnityEngine;
using Views;

namespace Db.Prefabs
{
    [CreateAssetMenu(fileName = "PrefabData", menuName = "Data/PrefabData")]
    public class PrefabData : ScriptableObject
    {
        [field:SerializeField] public BrickView BrickView { get; private set; }
    }
}