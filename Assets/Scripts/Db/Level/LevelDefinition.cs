using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Db.Level
{
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
}