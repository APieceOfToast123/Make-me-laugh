using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicState
{
    public string name;
    protected StateMachine stateMachine;

    public BasicState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// 状态中进行的动作
    /// </summary>
    public virtual void Action() { }

    /// <summary>
    /// 检测状态转换
    /// </summary>
    public virtual void Check() { }
    /// <summary>
    /// 退出状态
    /// </summary>
    public virtual void OnExit() { }
}
