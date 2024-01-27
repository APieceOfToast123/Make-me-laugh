using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class SCry : SSettled
{
    public SCry(PaientSM stateMachine) : base("Crying", stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Crying Enter");
    }

    public override void Check()
    {
        base.Check();
        if (pm.attributes.mood >= 40)
        {
            sm.ChangeState(StateID.Normal);
        }
    }
}
