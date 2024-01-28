using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class SComplete : SSettled
{
    public SComplete(PatientSM stateMachine) : base("Complete", stateMachine)
    {
        
    }

    int CompleteAward = 30;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Complete");
        pm.ClearTreatForce();
        ms.AddMoney(CompleteAward);
        EventManager.DeleteSettledOne(sm.OwnerGo.GetComponent<PaientManager>());
        
        pm.Destroy();
    }

    public override void Check()
    {
        base.Check();
    }
}
