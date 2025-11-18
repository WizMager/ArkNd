using Utils;

namespace Db.Level
{
    public readonly struct BrickSpawnInfo
    {
        public readonly int Row;
        public readonly int Column;
        public readonly BrickData Data;

        public BrickSpawnInfo(int row, int column, BrickData data)
        {
            Row = row;
            Column = column;
            Data = data;
        }
    }
}