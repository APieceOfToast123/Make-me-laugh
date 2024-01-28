using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

/// <summary>
/// 用来让角色什么都不要做的空状态
/// </summary>
public class SWaiting : SSettled
{
    public SWaiting(PatientSM stateMachine) : base("Waiting", stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void Check()
    {   
        
    }
}
