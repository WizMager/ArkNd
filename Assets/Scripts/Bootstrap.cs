using System.Collections.Generic;
using Ball.Impl;
using Core;
using Core.Interfaces;
using Db;
using Db.Game;
using Db.Prefabs;
using Platform;
using Services.Bricks;
using Services.Bricks.Impl;
using Services.Input;
using Services.Input.Impl;
using UnityEngine;
using Views;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private PrefabData _prefabData;
    [SerializeField] private GameData _gameData;

    [SerializeField] private BallView _ball;
    [SerializeField] private PlatformView _platform;
    
    private IModulesHandler _modulesHandler;

    private IBricksService _bricksService;
    private IInputService _inputService;
    
    private void Awake()
    {
        List<IModule> modules = new();
        
        _bricksService = new BricksService(_levelData, _prefabData);
        _bricksService.BuildLevel(0);

        _inputService = new InputService();
        _inputService.Initialize();

        var ballMove = new BallReflectModule(_ball, _gameData, _platform);
        modules.Add(ballMove);

        var platformMoveModule = new PlatformMoveModule(_inputService, _platform, _gameData);
        modules.Add(platformMoveModule);
        
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