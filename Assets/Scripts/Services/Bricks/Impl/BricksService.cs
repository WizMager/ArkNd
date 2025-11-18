using System;
using System.Collections.Generic;
using Db.Level;
using Db.Prefabs;
using UnityEngine;
using UnityEngine.Pool;
using Views;
using Object = UnityEngine.Object;

namespace Services.Bricks.Impl
{
    public class BricksService : IBricksService
    {
        private readonly LevelData _database;
        private readonly Transform _root;
        private readonly List<BrickView> _spawnedBricks = new();
        private readonly PrefabData _prefabData;
        private readonly ObjectPool<BrickView> _brickPool;
        
        public event Action OnBricksDestroyed;
        public IReadOnlyList<BrickView> SpawnedBricks => _spawnedBricks;

        public BricksService(LevelData database, PrefabData prefabData, Transform root = null)
        {
            _database = database;
            _prefabData = prefabData;
            _root = root != null ? root : new GameObject("BricksRoot").transform;
            _brickPool = new ObjectPool<BrickView>(CreateBrick, OnBrickTaken, OnBrickReleased);
        }

        public void BuildLevel(int levelIndex)
        {
            ClearField();
            var level = _database.GetLevel(levelIndex);

            foreach (var info in level.EnumerateBricks())
            {
                var instance = _brickPool.Get();
                var transform = instance.transform;
                transform.SetParent(_root);
                transform.localPosition = level.GetLocalPosition(info.Column, info.Row);
                instance.Initialize(info.Data);
                _spawnedBricks.Add(instance);
            }
        }

        public void ClearField()
        {
            for (var i = _spawnedBricks.Count - 1; i >= 0; i--)
            {
                var brick = _spawnedBricks[i];
                ReleaseBrick(brick);
            }

            _spawnedBricks.Clear();
        }

        public void ReleaseBrick(BrickView brickView)
        {
            if (brickView == null)
            {
                return;
            }

            _spawnedBricks.Remove(brickView);
            _brickPool.Release(brickView);
            if (_spawnedBricks.Count <= 0)
            {
                OnBricksDestroyed?.Invoke();
            }
        }

#region PoolMethods
        
        private BrickView CreateBrick()
        {
            var instance = Object.Instantiate(_prefabData.BrickView, _root);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private static void OnBrickTaken(BrickView view)
        {
            view.gameObject.SetActive(true);
        }

        private void OnBrickReleased(BrickView view)
        {
            view.gameObject.SetActive(false);
            view.transform.SetParent(_root);
        }
        
#endregion
        
    }
}