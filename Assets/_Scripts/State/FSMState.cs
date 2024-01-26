public abstract class FSMState
{
    // 所属的状态机
    protected FSMSystem m_FSM = null;
    protected FSMState(FSMSystem fsm)
    {
        m_FSM = fsm;
    }
    /// <summary>
    /// 进入状态
    /// </summary>
    public virtual void OnEnter() { }
    /// <summary>
    /// 状态中进行的动作
    /// </summary>
    public abstract void Action();
    /// <summary>
    /// 检测状态转换
    /// </summary>
    public abstract void Check();
    /// <summary>
    /// 退出状态
    /// </summary>
    public virtual void OnExit() { }
}
