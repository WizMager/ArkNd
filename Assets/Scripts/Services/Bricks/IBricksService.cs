using System;
using System.Collections.Generic;
using Views;

namespace Services.Bricks
{
    public interface IBricksService
    {
        event Action OnBricksDestroyed;
        IReadOnlyList<BrickView> SpawnedBricks { get; }
        
        void BuildLevel(int levelIndex);
        void ClearField();
        void ReleaseBrick(BrickView brickView);
    }
}