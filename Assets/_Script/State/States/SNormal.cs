using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class SNormal : SSettled
{
    public SNormal(PaientSM stateMachine) : base("Normal", stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Normal Enter");
    }

    public override void Check()
    {
        base.Check();
        if (pm.attributes.mood >= 70)
        {
            sm.ChangeState(StateID.Laugh);
        }else if (pm.attributes.mood <= 40)
        {
            sm.ChangeState(StateID.Cry);
        }
    }
}
