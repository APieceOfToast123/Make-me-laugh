using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class SFail : SSettled
{
    public SFail(PatientSM stateMachine) : base("Fail", stateMachine)
    {
        
    }
    int LosePunish = 15;


    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Fail");
        pm.ClearTreatForce();
        ms.ReduceMoney(LosePunish);
        EventManager.DeleteSettledOne(sm.OwnerGo.GetComponent<PaientManager>());
        
        pm.Destroy();
    }

    public override void Check()
    {
        base.Check();
    }
}
