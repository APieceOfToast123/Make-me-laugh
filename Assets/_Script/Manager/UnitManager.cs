using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏基本单位的基类
/// </summary>
public class UnitManager : MonoBehaviour
{
    public GameManager gm { get; set; }

    private void Awake()
    {
        GameManager.OnBeforeStateChanged += BeforeStateChanged;
        GameManager.OnAfterStateChanged += AfterStateChanged;
    }


    private void OnDestroy()
    {
        GameManager.OnBeforeStateChanged -= BeforeStateChanged;
        GameManager.OnAfterStateChanged -= AfterStateChanged;
    }

    private void BeforeStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Starting:
                HandleBeforeStarting();
                break;
            
        }
    }

    private void AfterStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Starting:
                HandleAfterStarting();
                break;
            
        }
    }
    
    public virtual void HandleBeforeStarting() { }
    public virtual void HandleBeforeRunning() { }
    public virtual void HandleBeforePausing() { }
    
    public virtual void HandleAfterStarting() { }
}
