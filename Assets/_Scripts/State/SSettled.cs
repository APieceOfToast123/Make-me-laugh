using System.Collections;
using System.Collections.Generic;
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
        changeMoodTimer = interval;
    }

    public override void Action()
    {
        base.Action();

        // 更新计时器
        changeMoodTimer -= Time.deltaTime;
        if (changeMoodTimer <= 0)
        {
            pm.changeMood(pm.attributes.effectFactors);
            changeMoodTimer = interval;
        }
    }
    
}
