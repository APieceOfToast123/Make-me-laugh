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

    protected void Awake()
    {
        GameManager.OnBeforeStateChanged += BeforeStateChanged;
        GameManager.OnAfterStateChanged += AfterStateChanged;
    }


    protected void OnDestroy()
    {
        GameManager.OnBeforeStateChanged -= BeforeStateChanged;
        GameManager.OnAfterStateChanged -= AfterStateChanged;
    }

    protected void BeforeStateChanged(GameState newState)
    {
        Debug.Log("Before");
        switch (newState)
        {
            case GameState.Starting:
                HandleBeforeStarting();
                break;
            case GameState.Running:
                HandleBeforeRunning();
                break;
            
        }
    }

    protected void AfterStateChanged(GameState newState)
    {
        Debug.Log("Unit Enter");
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
