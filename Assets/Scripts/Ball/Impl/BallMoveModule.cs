using System;
using Core.Interfaces;
using Db.Game;
using UnityEngine;
using Views;

namespace Ball.Impl
{
    public class BallMoveModule : IAwakable, IStartable, IFixedUpdatable, IDisposable
    {
        private readonly BallView _ball;
        private readonly Rigidbody2D _rigidbody;
        private readonly GameData _gameData;
        
        private Vector2 _lastVelocity;

        public BallMoveModule(BallView ball, GameData gameData)
        {
            _ball = ball;
            _rigidbody = ball.Rigidbody;
            _gameData = gameData;
            
            _ball.OnTouch += OnTouched;
        }

        private void OnTouched(Vector2 normal)
        {
            var direction = _lastVelocity.normalized;
            var reflected = Vector2.Reflect(direction, normal);

            _rigidbody.linearVelocity = reflected * _gameData.StartBallSpeed;
            _lastVelocity = _rigidbody.linearVelocity;
        }

        public void Awake()
        {
            _lastVelocity = Vector2.up * _gameData.StartBallSpeed;
        }

        public void Start()
        {
            _rigidbody.linearVelocity = _lastVelocity;
        }
        
        public void FixedUpdate()
        {
            _lastVelocity = _rigidbody.linearVelocity;
        }

        public void Dispose()
        {
            _ball.OnTouch -= OnTouched;
        }
    }
}