using System;
using System.Collections.Generic;
using Core.Interfaces;
using Db.Game;
using Services.Bricks;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

namespace Bricks.Impl
{
    public class BricksModule : IBricksModule, IAwakable, IStartable
    {
        private readonly IBricksService _bricksService;
        private readonly Dictionary<BrickView, int> _bricksHealth = new();
        private readonly GameData _gameData;

        private int _powerUpReadyCounter;
        
        public Action<Vector2> OnPowerUp { get; set; }
        
        public BricksModule(
            IBricksService bricksService, 
            GameData gameData
        )
        {
            _bricksService = bricksService;
            _gameData = gameData;
        }

        public void Awake()
        {
            _bricksService.BuildLevel(0);
        }
        
        public void Start()
        {
            _powerUpReadyCounter = _gameData.DestroyBlockCountPowerUp;
            
            foreach (var brick in _bricksService.SpawnedBricks)
            {
                _bricksHealth.Add(brick, brick.HitPoints);
                brick.OnBrickTouched += OnBrickTouched;
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
            
            Object.Destroy(brick.gameObject);
        }
    }
}