using System;
using Core.Interfaces;
using Db.Game;
using Services.Input;
using UnityEngine;
using Views;

namespace Ball.Impl
{
    public class AttackModule : IStartable, IDisposable
    {
        private readonly BallView _ball;
        private readonly IInputService _inputService;
        private readonly LoseLineView _loseLineView;
        private readonly GameData _gameData;

        private bool _isBallMove;
        private bool _isAttack;

        public AttackModule(
            BallView ball, 
            IInputService inputService, 
            LoseLineView loseLineView, 
            GameData gameData
        )
        {
            _ball = ball;
            _inputService = inputService;
            _loseLineView = loseLineView;
            _gameData = gameData;
        }

        public void Start()
        {
            _inputService.OnAttack += OnAttackHandle;
            _loseLineView.OnLose += OnLost;
        }
        
        private void OnAttackHandle(bool isAttack)
        {
            if (!_isBallMove && _isAttack)
            {
                _ball.transform.SetParent(null);
                _ball.Rigidbody.linearVelocity = Vector2.up * _gameData.DefaultBallSpeed;

                _isBallMove = true;
            }

            _isAttack = isAttack;
        }
        
        private void OnLost()
        {
            _isBallMove = false;
        }

        public void Dispose()
        {
            _inputService.OnAttack -= OnAttackHandle;
            _loseLineView.OnLose -= OnLost;
        }
    }
}