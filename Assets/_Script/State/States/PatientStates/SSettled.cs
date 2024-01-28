using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class SSettled : BasicState
{
    private float changeMoodTimer;
    public float interval = .5f; // 时间间隔
    protected PatientSM sm;
    protected PaientManager pm;
    protected MoneySystem ms;
    
    public SSettled(string name, PatientSM stateMachine) : base(name, stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        sm = (PatientSM) stateMachine;
        pm = sm.OwnerGo.GetComponent<PaientManager>();
        ms = MoneySystem.Instance;
        
        //TODO: 之后可能需要平衡一下数值
        switch (sm.CurrentStateID)
        {
            case StateID.Laugh:
                pm.patientAttributes.effectFactors = .6f;
                break;
            case StateID.Normal:
                pm.patientAttributes.effectFactors = 0;
                break;
            case StateID.Cry:
                pm.patientAttributes.effectFactors = -.8f;
                break;
        }
        
        EventManager.CallCheckEffect();
    }

    public override void Action()
    {
        base.Action();
    }

    public override void Check()
    {
        base.Check();
        
        //mood 100 - 70 => laugh; 70-40 => normal; 40-0 => car;
    }

    public override void OnExit()
    {
        base.OnExit();
        
        //把之前的数值还原回去
        switch (sm.CurrentStateID)
        {
            case StateID.Laugh:
                pm.patientAttributes.effectFactors = -1.5f;
                break;
            case StateID.Normal:
                pm.patientAttributes.effectFactors = 0;
                break;
            case StateID.Cry:
                pm.patientAttributes.effectFactors = 2f;
                break;
        }
        EventManager.CallCheckEffect();
    }
}
