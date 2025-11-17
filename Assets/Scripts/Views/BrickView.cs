using UnityEngine;
using Utils;

namespace Views
{
    public class BrickView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        
        private int _hitPoints;
        private MaterialPropertyBlock _propertyBlock;

        public int HitPoints => _hitPoints;

        private void Awake()
        {
            _propertyBlock ??= new MaterialPropertyBlock();
        }

        public void Initialize(BrickData data)
        {
            _hitPoints = data.HitPoints;

            if (_renderer == null)
            {
                return;
            }

            _renderer.color = data.Color;
        }
    }
}