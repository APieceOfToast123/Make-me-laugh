using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class SSettled : BasicState
{
    private float changeMoodTimer;
    public float interval = .5f; // 时间间隔
    protected PaientSM sm;
    protected PaientManager pm;
    
    public SSettled(string name, PaientSM stateMachine) : base(name, stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        sm = (PaientSM) stateMachine;
        pm = sm.OwnerGo.GetComponent<PaientManager>();
        
        //TODO: 之后可能需要平衡一下数值
        switch (sm.CurrentStateID)
        {
            case StateID.Laugh:
                pm.attributes.effectFactors = 1.5f;
                break;
            case StateID.Normal:
                pm.attributes.effectFactors = 0;
                break;
            case StateID.Cry:
                pm.attributes.effectFactors = -2f;
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
                pm.attributes.effectFactors = -1.5f;
                break;
            case StateID.Normal:
                pm.attributes.effectFactors = 0;
                break;
            case StateID.Cry:
                pm.attributes.effectFactors = 2f;
                break;
        }
        EventManager.CallCheckEffect();
    }
}
