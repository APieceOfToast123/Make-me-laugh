using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class StateMachine
{
    //private BasicState currentState;
    
    /// <summary>
    /// 状态机所属游戏物体
    /// </summary>
    public GameObject OwnerGo { get; private set; }
    public Animation Ani { get; private set; }
    
    private Dictionary<StateID, BasicState> m_StateMap = new Dictionary<StateID, BasicState>();
    public StateMachine(GameObject ownerGo)
    {
        OwnerGo = ownerGo;
        Ani = OwnerGo.GetComponent<Animation>();
    }

    /// <summary>
    /// 当前状态ID
    /// </summary>
    public StateID CurrentStateID { get; private set; }
    /// <summary>
    /// 当前状态
    /// </summary>
    public BasicState CurrentState { get; private set; }

    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="id">添加的状态ID</param>
    /// <param name="state">对应的状态对象</param>
    public void AddState(StateID id, BasicState state)
    {
        // 如果当前状态为空，就设置为默认状态
        if (CurrentState == null)
        {
            CurrentStateID = id;
            CurrentState = state;
            CurrentState.OnEnter();
        }
        if (m_StateMap.ContainsKey(id))
        {
            Debug.LogErrorFormat("状态ID:{0}已经存在，不能重复添加！", id);
            return;
        }
        m_StateMap.Add(id, state);
    }

    /// <summary>
    /// 移除状态
    /// </summary>
    /// <param name="id">要移除的状态ID</param>
    public void RemoveState(StateID id)
    {
        if (!m_StateMap.ContainsKey(id))
        {
            Debug.LogWarningFormat("状态ID:{0}不存在，不需要移除", id);
            return;
        }
        m_StateMap.Remove(id);
    }

    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="id">需要转换到的目标状态ID</param>
    public void ChangeState(StateID id)
    {
        if (id == CurrentStateID) return;//和之前状态相同之前退出
        if (!m_StateMap.ContainsKey(id))
        {
            Debug.LogErrorFormat("状态ID:{0}不存在！", id);
            return;
        }
        if (CurrentState != null)
            CurrentState.OnExit();
        CurrentStateID = id;
        CurrentState = m_StateMap[id];
        CurrentState.OnEnter();
    }

    /// <summary>
    /// 更新，在状态机持有者物体的Update中调用
    /// </summary>
    public void Update()
    {
        CurrentState.Action();
        CurrentState.Check();
    }

    //TODO: 这段代码在此处是无效的
    private void OnGUI()
    {
        string content = CurrentState != null ? CurrentState.name : "(no current state)";
        
        GUI.color = Color.black;
        // 将角色的世界坐标转换为屏幕坐标
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(OwnerGo.transform.position);
        // 设置 GUI 文本的位置
        screenPosition.y = Screen.height - screenPosition.y; // 注意 Unity 屏幕坐标的 Y 轴是从下往上的
        screenPosition.y -= 10 ; // 在角色的正下方 10 像素处显示

        // 绘制 GUI 文本
        GUI.Label(new Rect(screenPosition.x - 50, screenPosition.y - 25, 100, 50), "State: " + CurrentState.name);
    }
}
