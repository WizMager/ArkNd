using System;
using Core.Interfaces;
using Services.Bricks;
using UnityEngine;
using Views;

namespace Lose
{
    public class WinLoseModule : IStartable, IDisposable
    {
        private readonly LoseLineView _loseLineView;
        private readonly BallView _ballView;
        private readonly PlatformView _platformView;
        private readonly IBricksService _bricksService;

        public WinLoseModule(
            LoseLineView loseLineView, 
            BallView ballView, 
            PlatformView platformView, 
            IBricksService bricksService
        )
        {
            _loseLineView = loseLineView;
            _ballView = ballView;
            _platformView = platformView;
            _bricksService = bricksService;
        }

        public void Start()
        {
            _loseLineView.OnLose += OnLost;
            _bricksService.OnBricksDestroyed += OnWin;
        }

        private void OnLost()
        {
            StopAndAttachBall();
        }

        private void OnWin()
        {
            StopAndAttachBall();
        }

        private void StopAndAttachBall()
        {
            _ballView.Rigidbody.linearVelocity = Vector2.zero;
            _ballView.transform.position = _platformView.BallPosition.position;
            _ballView.transform.SetParent(_platformView.BallPosition);
        }
        
        public void Dispose()
        {
            _loseLineView.OnLose -= OnLost;
            _bricksService.OnBricksDestroyed -= OnWin;
        }
    }
}