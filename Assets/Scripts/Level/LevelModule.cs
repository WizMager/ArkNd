using Core.Interfaces;
using Db;
using Services.Bricks;

namespace Level
{
    public class LevelModule : IStartable
    {
        private readonly IBricksService _bricksService;
        private readonly LevelData _levelData;
        
        private int _level;

        public LevelModule(
            IBricksService bricksService, 
            LevelData levelData
        )
        {
            _bricksService = bricksService;
            _levelData = levelData;
        }

        public void Start()
        {
            _bricksService.OnBricksDestroyed += OnBricksDestroyed;
        }

        private void OnBricksDestroyed()
        {
            _bricksService.ClearField();
            _level++;

            if (_level >= _levelData.Levels.Count - 1)
            {
                _level = _levelData.Levels.Count - 1;
            }
            
            _bricksService.BuildLevel(_level);
        }
    }
}