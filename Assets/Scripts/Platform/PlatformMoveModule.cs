using Core.Interfaces;
using Db.Game;
using Services.Input;
using UnityEngine;
using Views;

namespace Platform
{
    public class PlatformMoveModule : IUpdatable
    {
        private readonly IInputService _inputService;
        private readonly Transform _platformTransform;
        private readonly GameData _gameData;

        public PlatformMoveModule(
            IInputService inputService, 
            PlatformView platform, 
            GameData gameData
        )
        {
            _inputService = inputService;
            _gameData = gameData;
            _platformTransform = platform.transform;
        }
        
        public void Update()
        {
            if (!_inputService.IsMove)
                return;
            
            var direction = _inputService.MoveDirection;
            _platformTransform.Translate(Vector3.right * direction * Time.deltaTime * _gameData.PlatformSpeed);
        }
    }
}