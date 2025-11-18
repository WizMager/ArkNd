using System;
using UnityEngine;
using Utils;

namespace Db.Level
{
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
}