using System;
using UnityEngine;
using Utils;

namespace Views
{
    public class BrickView : MonoBehaviour
    {
        public Action<BrickView> OnBrickTouched;
        
        [SerializeField] private SpriteRenderer _renderer;

        public int HitPoints { get; private set; }

        public void Initialize(BrickData data)
        {
            HitPoints = data.HitPoints;

            if (_renderer == null)
            {
                return;
            }

            _renderer.color = data.Color;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ball"))
                return;
            
            OnBrickTouched?.Invoke(this);
        }
    }
}