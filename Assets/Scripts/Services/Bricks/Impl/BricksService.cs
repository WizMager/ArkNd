using System.Collections.Generic;
using Db;
using Db.Prefabs;
using UnityEngine;
using Views;

namespace Services.Bricks.Impl
{
    public class BricksService : IBricksService
    {
        private readonly LevelData _database;
        private readonly Transform _root;
        private readonly List<Brick> _spawnedBricks = new();
        private readonly PrefabData _prefabData;

        public BricksService(LevelData database, PrefabData prefabData, Transform root = null)
        {
            _database = database;
            _prefabData = prefabData;
            _root = root != null ? root : new GameObject("BricksRoot").transform;
        }

        public IReadOnlyList<Brick> SpawnedBricks => _spawnedBricks;

        public void BuildLevel(int levelIndex)
        {
            ClearField();
            var level = _database.GetLevel(levelIndex);

            foreach (var info in level.EnumerateBricks())
            {
                var instance = Object.Instantiate(_prefabData.Brick, _root);
                instance.transform.localPosition = level.GetLocalPosition(info.Column, info.Row);
                instance.Initialize(info.Data);
                _spawnedBricks.Add(instance);
            }
        }

        public void ClearField()
        {
            foreach (var brick in _spawnedBricks)
            {
                if (brick != null)
                {
                    Object.Destroy(brick.gameObject);
                }
            }

            _spawnedBricks.Clear();
        }
    }
}