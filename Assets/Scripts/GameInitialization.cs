﻿using Systems;
using Ui;
using UnityEngine;

public class GameInitialization : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private GameObject _uiRoot;
        
        
    private void Awake()
    {
        BindSystems();
            
        ServiceLocator.Init();
    }

    private void BindSystems()
    {
        ServiceLocator.Add<CurrencySystem>();
        ServiceLocator.Add<SkillsLearningSystem>();
        ServiceLocator.Add(new MainHudController(_uiRoot));
    }

    private void Start()
    {
        ServiceLocator.Start();
    }

    private void Update()
    {
        ServiceLocator.GlobalTick(Time.deltaTime);
    }
}