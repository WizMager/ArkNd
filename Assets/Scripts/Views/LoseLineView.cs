using System;
using UnityEngine;

namespace Views
{
    public class LoseLineView : MonoBehaviour
    {
        public Action OnLose;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Ball"))
                return;
            
            OnLose?.Invoke();
        }
    }
}