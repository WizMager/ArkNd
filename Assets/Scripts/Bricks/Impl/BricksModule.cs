using System.Collections.Generic;
using Core.Interfaces;
using Services.Bricks;
using UnityEngine;
using Views;

namespace Bricks.Impl
{
    public class BricksModule : IBricksModule, IStartable
    {
        private readonly IBricksService _bricksService;
        private readonly Dictionary<BrickView, int> _bricksHealth = new();

        public BricksModule(IBricksService bricksService)
        {
            _bricksService = bricksService;
        }

        public void Start()
        {
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
            
            if (health > 0) 
                return;
            
            _bricksHealth.Remove(brick);
            
            Object.Destroy(brick.gameObject);
        }
    }
}