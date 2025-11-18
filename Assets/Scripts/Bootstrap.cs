using System;
using System.Collections.Generic;
using Ball.Impl;
using Bricks;
using Bricks.Impl;
using Core;
using Core.Interfaces;
using Db;
using Db.Game;
using Db.PowerUp;
using Db.Prefabs;
using Level;
using Lose;
using Platform;
using PowerUp;
using PowerUp.Impl;
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
    [SerializeField] private PowerUpData _powerUpData;

    [SerializeField] private BallView _ball;
    [SerializeField] private PlatformView _platform;
    [SerializeField] private LoseLineView _loseLineView;
    
    private IModulesHandler _modulesHandler;

    private IBricksModule _bricksModule;
    private IPowerUpModule _powerUpModule;
    
    private IBricksService _bricksService;
    private IInputService _inputService;

    private ForCoroutine _forCoroutine;
    
    private void Awake()
    {
        _forCoroutine = new GameObject("ForCoroutine").AddComponent<ForCoroutine>();
        
        List<IModule> modules = new();
        
        _bricksService = new BricksService(_levelData, _prefabData);

        _inputService = new InputService();
        _inputService.Initialize();
        
        var ballMove = new BallReflectModule(_ball, _gameData, _platform, _inputService, _loseLineView);
        modules.Add(ballMove);

        var bricksModule = new BricksModule(_bricksService, _gameData, _forCoroutine);
        _bricksModule = bricksModule;
        modules.Add(bricksModule);

        var powerUpModule = new PowerUpModule(_bricksModule, _prefabData, _powerUpData, _platform, _loseLineView, _gameData);
        _powerUpModule = powerUpModule;
        modules.Add(powerUpModule);
        
        var platformMoveModule = new PlatformMoveModule(_inputService, _platform, _gameData, _powerUpModule, _powerUpData);
        modules.Add(platformMoveModule);
        
        var attackModule = new AttackModule(_ball, _inputService, _loseLineView, _gameData);
        modules.Add(attackModule);
        
        var loseModule = new WinLoseModule(_loseLineView, _ball, _platform, _bricksService);
        modules.Add(loseModule);
        
        var levelModule = new LevelModule(_bricksService, _levelData);
        modules.Add(levelModule);
        
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
    
    private void OnDestroy()
    {
        _modulesHandler?.Dispose();
        _inputService?.Dispose();
        
        if (_forCoroutine != null)
        {
            Destroy(_forCoroutine.gameObject);
        }
    }
}