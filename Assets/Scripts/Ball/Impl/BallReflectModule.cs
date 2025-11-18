using System;
using Core.Interfaces;
using Db.Game;
using Services.Input;
using UnityEngine;
using Views;

namespace Ball.Impl
{
    public class BallReflectModule : IAwakable, IFixedUpdatable, IDisposable
    {
        private readonly BallView _ball;
        private readonly Rigidbody2D _rigidbody;
        private readonly GameData _gameData;
        private readonly PlatformView _platformView;
        private readonly IInputService _inputService;
        private readonly LoseLineView _loseLineView;
        
        private Vector2 _lastVelocity;
        private float _halfPlatformWidth;
        private bool _isReadyForReflect;

        public BallReflectModule(
            BallView ball, 
            GameData gameData, 
            PlatformView platformView, 
            IInputService inputService, 
            LoseLineView loseLineView
        )
        {
            _ball = ball;
            _rigidbody = ball.Rigidbody;
            _gameData = gameData;
            _platformView = platformView;
            _inputService = inputService;
            _loseLineView = loseLineView;

            _ball.OnTouch += OnReflected;
            _platformView.OnPlatformReflect += OnPlatformReflected;
            _inputService.OnAttack += OnAttack;
            _loseLineView.OnLose += OnLost;
        }

        private void OnReflected(Vector2 normal)
        {
            var direction = _lastVelocity.normalized;
            var reflected = Vector2.Reflect(direction, normal);

            _rigidbody.linearVelocity = reflected * _gameData.DefaultBallSpeed;
            _lastVelocity = _rigidbody.linearVelocity;
        }

        private void OnPlatformReflected(Vector2 contactPoint)
        {
            if (!_isReadyForReflect)
                return;
            
            var localContactPoint = _platformView.transform.InverseTransformPoint(contactPoint);
            var relativePosition = localContactPoint.x / _halfPlatformWidth;
            relativePosition = Mathf.Clamp(relativePosition, -1f, 1f);
            
            var distanceFromCenter = Mathf.Abs(relativePosition);
            var bounceAngle = Mathf.Lerp(_gameData.MinBounceAngle, _gameData.MaxBounceAngle, distanceFromCenter);
            
            if (relativePosition < 0f)
            {
                bounceAngle = -bounceAngle;
            }
            
            var radians = bounceAngle * Mathf.Deg2Rad;
            var directionX = Mathf.Sin(radians);
            var directionY = Mathf.Cos(radians);
            
            var bounceDirection = new Vector2(directionX, directionY).normalized;
            
            _rigidbody.linearVelocity = bounceDirection * _gameData.DefaultBallSpeed;
            _lastVelocity = _rigidbody.linearVelocity;
        }
        
        private void OnAttack(bool isAttack)
        {
            _isReadyForReflect = true;
        }
        
        private void OnLost()
        {
            _isReadyForReflect = false;
        }
        
        public void Awake()
        {
            _halfPlatformWidth = _platformView.Collider.bounds.extents.x;
        }
        
        public void FixedUpdate()
        {
            _lastVelocity = _rigidbody.linearVelocity;
        }

        public void Dispose()
        {
            _ball.OnTouch -= OnReflected;
            _platformView.OnPlatformReflect -= OnPlatformReflected;
            _inputService.OnAttack += OnAttack;
            _loseLineView.OnLose += OnLost;
        }
    }
}