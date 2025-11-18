using System;
using Core.Interfaces;
using UnityEngine;
using Views;

namespace Lose
{
    public class LoseModule : IStartable, IDisposable
    {
        private readonly LoseLineView _loseLineView;
        private readonly BallView _ballView;
        private readonly PlatformView _platformView;

        public LoseModule(
            LoseLineView loseLineView, 
            BallView ballView, 
            PlatformView platformView
        )
        {
            _loseLineView = loseLineView;
            _ballView = ballView;
            _platformView = platformView;
        }

        public void Start()
        {
            _loseLineView.OnLose += OnLost;
        }

        private void OnLost()
        {
            _ballView.Rigidbody.linearVelocity = Vector2.zero;
            _ballView.transform.position = _platformView.BallPosition.position;
            _ballView.transform.SetParent(_platformView.BallPosition);
        }

        public void Dispose()
        {
            _loseLineView.OnLose -= OnLost;
        }
    }
}