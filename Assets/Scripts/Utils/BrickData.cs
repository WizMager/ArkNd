using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public struct BrickData
    {
        [SerializeField] private Color _color;
        [SerializeField] private int _hitPoints;
        
        public Color Color => _color;
        public int HitPoints => _hitPoints;
    }
}