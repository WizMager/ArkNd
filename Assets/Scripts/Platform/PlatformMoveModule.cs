using System;
using Core.Interfaces;
using Db.Game;
using Db.PowerUp;
using PowerUp;
using Services.Input;
using UnityEngine;
using Views;

namespace Platform
{
    public class PlatformMoveModule : IStartable, IUpdatable, IDisposable
    {
        private readonly IInputService _inputService;
        private readonly Transform _platformTransform;
        private readonly GameData _gameData;
        private readonly IPowerUpModule _powerUpModule;
        private readonly PlatformView _platformView;
        private readonly PowerUpData _powerUpData;

        public PlatformMoveModule(
            IInputService inputService, 
            PlatformView platform, 
            GameData gameData, 
            IPowerUpModule powerUpModule, 
            PowerUpData powerUpData
        )
        {
            _inputService = inputService;
            _gameData = gameData;
            _powerUpModule = powerUpModule;
            _powerUpData = powerUpData;
            _platformTransform = platform.transform;
            _platformView = platform;
        }
        
        public void Start()
        {
            _powerUpModule.OnPowerUpCollected += OnPowerUpCollected;
        }

        private void OnPowerUpCollected(EPowerUp powerUp)
        {
            switch (powerUp)
            {
                case EPowerUp.AddPlatform:
                    ChangePlatformSize(true);
                    break;
                case EPowerUp.ReducePlatform:
                    ChangePlatformSize(false);
                    break;
            }
        }

        private void ChangePlatformSize(bool isIncrease)
        {
            var value = isIncrease ? _powerUpData.PlatformSizeChange : -_powerUpData.PlatformSizeChange;
            _platformView.ChangeSize(value);
        }
        
        public void Update()
        {
            if (!_inputService.IsMove)
                return;
            
            var direction = _inputService.MoveDirection;
            
            if (Mathf.Approximately(direction, 0f))
                return;

            var delta = Vector3.right * direction * Time.deltaTime * _gameData.PlatformSpeed;
            var targetPosition = _platformTransform.position + delta;

            targetPosition.x = Mathf.Clamp(targetPosition.x, GetLeftLimit(), GetRightLimit());
            _platformTransform.position = targetPosition;
        }

        private float GetLeftLimit()
        {
            return GetBoundaryLimit(_platformView.LeftBoundary, true);
        }

        private float GetRightLimit()
        {
            return GetBoundaryLimit(_platformView.RightBoundary, false);
        }
        
        private float GetBoundaryLimit(BoxCollider2D boundary, bool isLeft)
        {
            if (boundary == null)
                return isLeft ? float.NegativeInfinity : float.PositiveInfinity;

            var halfWidth = _platformView.Collider.bounds.extents.x;
            var boundaryX = isLeft ? boundary.bounds.max.x : boundary.bounds.min.x;
            return isLeft ? boundaryX + halfWidth : boundaryX - halfWidth;
        }
        
        public void Dispose()
        {
            _powerUpModule.OnPowerUpCollected -= OnPowerUpCollected;
        }
    }
}