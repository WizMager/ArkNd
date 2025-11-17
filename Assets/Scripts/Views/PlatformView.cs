using System;
using UnityEngine;

namespace Views
{
    public class PlatformView : MonoBehaviour
    {
        public Action<Vector2> OnPlatformReflect;
        
        [field:SerializeField] public BoxCollider2D Collider { get; private set; }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ball"))
                return;
            
            OnPlatformReflect?.Invoke(other.transform.position);
        }
    }
}