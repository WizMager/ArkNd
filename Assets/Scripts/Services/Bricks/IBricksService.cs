using System.Collections.Generic;
using Views;

namespace Services.Bricks
{
    public interface IBricksService
    {
        IReadOnlyList<Brick> SpawnedBricks { get; }
        void BuildLevel(int levelIndex);
        void ClearField();
    }
}