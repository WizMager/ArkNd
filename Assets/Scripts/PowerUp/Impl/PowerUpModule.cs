using System;
using System.Collections.Generic;
using Bricks;
using Core.Interfaces;
using Db.Game;
using Db.PowerUp;
using Db.Prefabs;
using UnityEngine;
using UnityEngine.Pool;
using Views;
using Object = UnityEngine.Object;

namespace PowerUp.Impl
{
    public class PowerUpModule : IPowerUpModule, IStartable, IUpdatable, IDisposable
    {
        private readonly IBricksModule _bricksModule;
        private readonly PrefabData _prefabData;
        private readonly PowerUpData _powerUpData;
        private readonly PlatformView _platformView;
        private readonly LoseLineView _loseLineView;
        private readonly float _fallSpeed;
        private readonly Transform _powerUpRoot;
        private readonly ObjectPool<PowerUpView> _powerUpPool;
        private readonly List<ActivePowerUp> _activePowerUps = new();

        public Action<EPowerUp> OnPowerUpCollected { get; set; }

        public PowerUpModule(
            IBricksModule bricksModule,
            PrefabData prefabData,
            PowerUpData powerUpData,
            PlatformView platformView,
            LoseLineView loseLineView,
            GameData gameData
        )
        {
            _bricksModule = bricksModule;
            _prefabData = prefabData;
            _powerUpData = powerUpData;
            _platformView = platformView;
            _loseLineView = loseLineView;
            _fallSpeed = Mathf.Max(0.1f, gameData.PowerUpFallSpeed);

            _powerUpRoot = new GameObject("PowerUpsRoot").transform;
            _powerUpPool = new ObjectPool<PowerUpView>(
                CreatePowerUp,
                OnPowerUpTaken,
                OnPowerUpReleased);
        }

#region PoolMethods

        private PowerUpView CreatePowerUp()
        {
            var instance = Object.Instantiate(_prefabData.PowerUpView, _powerUpRoot);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private static void OnPowerUpTaken(PowerUpView view)
        {
            view.gameObject.SetActive(true);
        }

        private void OnPowerUpReleased(PowerUpView view)
        {
            view.gameObject.SetActive(false);
            view.transform.SetParent(_powerUpRoot);
        }

#endregion
        
        public void Start()
        {
            _bricksModule.OnPowerUp += OnPowerUpHandle;
        }

        public void Update()
        {
            if (_activePowerUps.Count == 0)
            {
                return;
            }

            var delta = Vector3.down * (_fallSpeed * Time.deltaTime);
            var platformBounds = _platformView.Collider.bounds;

            for (var i = _activePowerUps.Count - 1; i >= 0; i--)
            {
                var entry = _activePowerUps[i];
                var viewTransform = entry.View.transform;

                viewTransform.position += delta;
                var position = viewTransform.position;

                if (platformBounds.Contains(position))
                {
                    OnPowerUpCollected?.Invoke(entry.Type);
                    ReleasePowerUp(i);
                    continue;
                }

                var loseLineY = _loseLineView.transform.position.y;
                
                if (position.y <= loseLineY)
                {
                    ReleasePowerUp(i);
                }
            }
        }

        private void OnPowerUpHandle(Vector2 spawnPosition)
        {
            if (_prefabData.PowerUpView == null)
            {
                return;
            }

            if (!_powerUpData.TryGetRandomPowerUp(out var pair))
            {
                return;
            }

            var view = _powerUpPool.Get();
            view.transform.position = spawnPosition;
            view.Initialize(pair);

            _activePowerUps.Add(new ActivePowerUp(view, pair.PowerUp));
        }

        private void ReleasePowerUp(int index)
        {
            var entry = _activePowerUps[index];
            _activePowerUps.RemoveAt(index);
            _powerUpPool.Release(entry.View);
        }
        
        public void Dispose()
        {
            _bricksModule.OnPowerUp -= OnPowerUpHandle;
        }
    }
}