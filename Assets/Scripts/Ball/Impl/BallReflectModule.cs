using System;
using Core.Interfaces;
using Db.Game;
using Services.Input;
using UnityEngine;
using Views;
using Random = UnityEngine.Random;

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
        private Vector2 _collisionVelocity;
        private int _collisionFrame = -1;
        private float _halfPlatformWidth;
        private bool _isReadyForReflect;
        private int _perpendicularReflectionCount;

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
            if (_ball.transform.parent != null)
                return;
            
            if (_collisionFrame != Time.frameCount)
            {
                _collisionFrame = Time.frameCount;
                _collisionVelocity = _lastVelocity.sqrMagnitude > 0.0001f
                    ? _lastVelocity
                    : _rigidbody.linearVelocity.sqrMagnitude > 0.0001f ? _rigidbody.linearVelocity : Vector2.up;
            }

            var direction = _collisionVelocity.normalized;
            
            var dotProduct = Mathf.Abs(Vector2.Dot(direction, normal));
            var isPerpendicular = dotProduct < _gameData.PerpendicularAngleThreshold;
            
            if (isPerpendicular)
            {
                _perpendicularReflectionCount++;
            }
            else
            {
                _perpendicularReflectionCount = 0;
            }
            
            var reflected = Vector2.Reflect(direction, normal);
            
            if (_perpendicularReflectionCount >= 2)
            {
                var deviationDegrees = Random.Range(-_gameData.DeviationAngle, _gameData.DeviationAngle);
                var rotation = Quaternion.Euler(0, 0, deviationDegrees);
                reflected = rotation * reflected;
                _perpendicularReflectionCount = 0;
            }
            
            var newVelocity = reflected * _gameData.DefaultBallSpeed;

            _collisionVelocity = newVelocity;
            _rigidbody.linearVelocity = newVelocity;
            _lastVelocity = newVelocity;
            _rigidbody.position += normal * _gameData.PenetrationOffset;
        }

        private void OnPlatformReflected(Vector2 contactPoint)
        {
            if (_ball.transform.parent != null)
                return;
            
            if (!_isReadyForReflect)
                return;
            
            _perpendicularReflectionCount = 0;
            
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
            _perpendicularReflectionCount = 0;
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
            _inputService.OnAttack -= OnAttack;
            _loseLineView.OnLose -= OnLost;
        }
    }
}