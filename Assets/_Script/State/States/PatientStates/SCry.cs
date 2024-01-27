using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class SCry : SSettled
{
    public SCry(PatientSM stateMachine) : base("Crying", stateMachine)
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
        if (pm.patientAttributes.mood <= 0)
        {
            sm.ChangeState(StateID.fail);
        }else if (pm.patientAttributes.mood >= 40)
        {
            sm.ChangeState(StateID.Normal);
        }
    }
}
