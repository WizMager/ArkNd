using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Db
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

    [Serializable]
    public class LevelDefinition
    {
        [SerializeField] private int _levelIndex;
        [SerializeField] private Vector2 _origin;
        [SerializeField] private Vector2 _cellSize;
        [SerializeField] private List<string> _rows = new();
        [SerializeField] private List<LegendEntry> _legend = new();

        public int LevelIndex => _levelIndex;
        public int RowsCount => _rows.Count;
        public int ColumnsCount => _rows.Count == 0 ? 0 : _rows[0].Length;
        public Vector2 Origin => _origin;
        public Vector2 CellSize => _cellSize;

        public IEnumerable<BrickSpawnInfo> EnumerateBricks()
        {
            if (_rows == null || _rows.Count == 0)
            {
                yield break;
            }

            for (var row = 0; row < _rows.Count; row++)
            {
                var rowString = _rows[row];
                for (var column = 0; column < rowString.Length; column++)
                {
                    var symbol = rowString[column];
                    if (!TryResolve(symbol, out var data))
                    {
                        continue;
                    }

                    yield return new BrickSpawnInfo(row, column, data);
                }
            }
        }

        public Vector3 GetLocalPosition(int column, int row)
        {
            var x = _origin.x + column * _cellSize.x;
            var y = _origin.y - row * _cellSize.y;
            return new Vector3(x, y, 0f);
        }

        private bool TryResolve(char symbol, out BrickData data)
        {
            foreach (var entry in _legend)
            {
                if (entry.Symbol == symbol)
                {
                    data = entry.Data;
                    return !entry.IsEmpty;
                }
            }

            data = default;
            return false;
        }
    }

    [Serializable]
    public class LegendEntry
    {
        [SerializeField] private char _symbol = '#';
        [SerializeField] private bool _isEmpty;
        [SerializeField] private BrickData _data;

        public char Symbol => _symbol;
        public bool IsEmpty => _isEmpty;
        public BrickData Data => _data;
    }

    public readonly struct BrickSpawnInfo
    {
        public readonly int Row;
        public readonly int Column;
        public readonly BrickData Data;

        public BrickSpawnInfo(int row, int column, BrickData data)
        {
            Row = row;
            Column = column;
            Data = data;
        }
    }
}

