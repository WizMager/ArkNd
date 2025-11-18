using System;
using Core.Interfaces;
using Db.Game;
using Services.Bricks;
using Services.Input;
using UnityEngine;
using Views;

namespace Ball.Impl
{
    public class AttackModule : IStartable, IDisposable
    {
        private readonly BallView _ball;
        private readonly IInputService _inputService;
        private readonly IBricksService _bricksService;
        private readonly LoseLineView _loseLineView;
        private readonly GameData _gameData;

        private bool _isBallMove;

        public AttackModule(
            BallView ball, 
            IInputService inputService, 
            LoseLineView loseLineView, 
            GameData gameData,
            IBricksService bricksService
        )
        {
            _ball = ball;
            _inputService = inputService;
            _loseLineView = loseLineView;
            _gameData = gameData;
            _bricksService = bricksService;
        }

        public void Start()
        {
            _inputService.OnAttack += OnAttackHandle;
            _loseLineView.OnLose += OnLost;
            _bricksService.OnBricksDestroyed += OnBricksDestroyed;
        }
        
        private void OnAttackHandle(bool isAttack)
        {
            if (!_isBallMove && isAttack)
            {
                _ball.transform.SetParent(null);
                _ball.Rigidbody.linearVelocity = Vector2.up * _gameData.DefaultBallSpeed;

                _isBallMove = true;
            }
        }
        
        private void OnLost()
        {
            _isBallMove = false;
        }

        private void OnBricksDestroyed()
        {
            StopBall();
        }
        
        private void StopBall()
        {
            _isBallMove = false;
        }

        public void Dispose()
        {
            _inputService.OnAttack -= OnAttackHandle;
            _loseLineView.OnLose -= OnLost;
            _bricksService.OnBricksDestroyed -= OnBricksDestroyed;
        }
    }
}