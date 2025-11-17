using System.Collections.Generic;
using Core;
using Core.Interfaces;
using Db;
using Db.Prefabs;
using Services.Bricks;
using Services.Bricks.Impl;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private PrefabData _prefabData;
    
    private IModulesHandler _modulesHandler;

    private IBricksService _bricksService;
    private void Awake()
    {
        _bricksService = new BricksService(_levelData, _prefabData);
        _bricksService.BuildLevel(0);
        
        List<IModule> modules = new();
        
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