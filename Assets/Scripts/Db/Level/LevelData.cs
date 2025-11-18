using System;
using System.Collections.Generic;
using UnityEngine;

namespace Db.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private List<LevelDefinition> _levels = new();

        public LevelDefinition GetLevel(int index)
        {
            if (_levels == null || _levels.Count == 0)
            {
                throw new InvalidOperationException("В базе уровней нет ни одной матрицы.");
            }

            if (index < 0 || index >= _levels.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Матрица с индексом {index} отсутствует. Доступно: {_levels.Count}");
            }

            return _levels[index];
        }

        public IReadOnlyList<LevelDefinition> Levels => _levels;
    }
}

