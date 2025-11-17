using System.Collections.Generic;
using Ball.Impl;
using Core;
using Core.Interfaces;
using Db;
using Db.Game;
using Db.Prefabs;
using Services.Bricks;
using Services.Bricks.Impl;
using UnityEngine;
using Views;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private PrefabData _prefabData;
    [SerializeField] private GameData _gameData;

    [SerializeField] private BallView _ball;
    
    private IModulesHandler _modulesHandler;

    private IBricksService _bricksService;
    private void Awake()
    {
        List<IModule> modules = new();
        
        _bricksService = new BricksService(_levelData, _prefabData);
        _bricksService.BuildLevel(0);

        var ballMove = new BallMoveModule(_ball, _gameData);
        modules.Add(ballMove);
        
        _modulesHandler = new ModulesHandler(modules);
        
        _modulesHandler.Awake();
    }

    private void Start()
    {
        _modulesHandler.Start();
    }
    
    private void FixedUpdate()
    {
        _modulesHandler.FixedUpdate();
    }
    
    private void Update()
    {
        _modulesHandler.Update();
    }
}