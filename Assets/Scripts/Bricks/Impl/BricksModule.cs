using System;
using System.Collections;
using System.Collections.Generic;
using Core.Interfaces;
using Db.Game;
using Services.Bricks;
using UnityEngine;
using Views;

namespace Bricks.Impl
{
    public class BricksModule : IBricksModule, IAwakable, IStartable, IDisposable
    {
        private readonly IBricksService _bricksService;
        private readonly Dictionary<BrickView, int> _bricksHealth = new();
        private readonly GameData _gameData;
        private readonly ForCoroutine _forCoroutine;

        private int _powerUpReadyCounter;
        
        public Action<Vector2> OnPowerUp { get; set; }
        
        public BricksModule(
            IBricksService bricksService, 
            GameData gameData, 
            ForCoroutine forCoroutine
        )
        {
            _bricksService = bricksService;
            _gameData = gameData;
            _forCoroutine = forCoroutine;
        }

        public void Awake()
        {
            _bricksService.BuildLevel(0);
        }
        
        public void Start()
        {
            _bricksService.OnBricksDestroyed += OnBricksDestroyed;
            _powerUpReadyCounter = _gameData.DestroyBlockCountPowerUp;
            
            foreach (var brick in _bricksService.SpawnedBricks)
            {
                RegisterBrick(brick);
            }
        }

        private void OnBricksDestroyed()
        {
            foreach (var brick in _bricksHealth.Keys)
            {
                brick.OnBrickTouched -= OnBrickTouched;
            }

            _bricksHealth.Clear();
            
            _forCoroutine.StartCoroutine(WaitAndAddSpawnedBricks());
        }

        private IEnumerator WaitAndAddSpawnedBricks()
        {
            yield return new WaitForSeconds(0.5f);
            
            foreach (var brick in _bricksService.SpawnedBricks)
            {
                RegisterBrick(brick);
            }
        }
        
        private void OnBrickTouched(BrickView brick)
        {
            var health = _bricksHealth[brick];
            health--;
            _bricksHealth[brick] = health;
            
            if (health > 0) 
                return;
            
            _powerUpReadyCounter--;
            
            if (_powerUpReadyCounter <= 0)
            {
                _powerUpReadyCounter = _gameData.DestroyBlockCountPowerUp;
                
                OnPowerUp?.Invoke(brick.transform.position);
            }
            
            _bricksHealth.Remove(brick);
            brick.OnBrickTouched -= OnBrickTouched;
            
            _bricksService.ReleaseBrick(brick);
        }

        private void RegisterBrick(BrickView brick)
        {
            if (brick == null) 
                return;
            
            _bricksHealth[brick] = brick.HitPoints;
            brick.OnBrickTouched += OnBrickTouched;
        }

        public void Dispose()
        {
            foreach (var brick in _bricksHealth.Keys)
            {
                brick.OnBrickTouched -= OnBrickTouched;
            }
        }
    }
}