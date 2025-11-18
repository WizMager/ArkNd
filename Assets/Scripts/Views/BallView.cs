using System;
using UnityEngine;

namespace Views
{
    public class BallView : MonoBehaviour
    {
        public Action<Vector2> OnTouch;

        private int _lastCollisionFrame = -1;

        [field:SerializeField] public Rigidbody2D Rigidbody { get; private set; }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Reflect"))
                return;

            if (_lastCollisionFrame == Time.frameCount)
                return;

            _lastCollisionFrame = Time.frameCount;
            
            OnTouch?.Invoke(other.contacts[0].normal);
        }
    }
}