using System;
using Core.Interfaces;
using Db.Game;
using UnityEngine;
using Views;

namespace Ball.Impl
{
    public class BallReflectModule : IAwakable, IStartable, IFixedUpdatable, IDisposable
    {
        private readonly BallView _ball;
        private readonly Rigidbody2D _rigidbody;
        private readonly GameData _gameData;
        private readonly PlatformView _platformView;
        
        private Vector2 _lastVelocity;
        private float _halfPlatformWidth;

        public BallReflectModule(
            BallView ball, 
            GameData gameData, 
            PlatformView platformView
        )
        {
            _ball = ball;
            _rigidbody = ball.Rigidbody;
            _gameData = gameData;
            _platformView = platformView;

            _ball.OnTouch += OnReflected;
            _platformView.OnPlatformReflect += OnPlatformReflected;
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
        
        public void Awake()
        {
            _lastVelocity = Vector2.up * _gameData.DefaultBallSpeed;
            _halfPlatformWidth = _platformView.Collider.bounds.extents.x;
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
            _ball.OnTouch -= OnReflected;
            _platformView.OnPlatformReflect -= OnPlatformReflected;
        }
    }
}