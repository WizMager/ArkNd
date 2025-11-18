using System;
using UnityEngine;

namespace Views
{
    public class PlatformView : MonoBehaviour
    {
        public Action<Vector2> OnPlatformReflect;
        
        [field:SerializeField] public Transform BallPosition { get; private set; }
        [field:SerializeField] public BoxCollider2D Collider { get; private set; }
        [field:SerializeField] public Collider2D LeftBoundary { get; private set; }
        [field:SerializeField] public Collider2D RightBoundary { get; private set; }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ball"))
                return;
            
            OnPlatformReflect?.Invoke(other.transform.position);
        }

        public void ChangeSize(float newSize)
        {
            var scale = transform.localScale;
            scale.x += newSize;
            
            if (scale.x < 1)
            {
                scale.x = 1;
            }

            if (scale.x > 5.5f)
            {
                scale.x = 5.5f;
            }
            
            transform.localScale = scale;
        }
    }
}