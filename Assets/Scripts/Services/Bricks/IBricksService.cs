using System.Collections.Generic;
using Views;

namespace Services.Bricks
{
    public interface IBricksService
    {
        IReadOnlyList<BrickView> SpawnedBricks { get; }
        void BuildLevel(int levelIndex);
        void ClearField();
    }
}