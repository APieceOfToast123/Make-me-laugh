using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class SLaugh : SSettled
{
    public SLaugh(PaientSM stateMachine) : base("Laughing", stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Laughing Enter");
    }

    public override void Check()
    {
        base.Check();
        if (pm.attributes.mood <= 70)
        {
            sm.ChangeState(StateID.Normal);
        }
    }
}
