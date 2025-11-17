using System.Collections.Generic;
using Core;
using Core.Interfaces;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private IModulesHandler _modulesHandler;
    private void Awake()
    {
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