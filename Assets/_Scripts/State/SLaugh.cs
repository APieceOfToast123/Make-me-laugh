using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLaugh : SSettled
{
    public SLaugh(PaientSM stateMachine) : base("Laughing", stateMachine)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Laugh");
        //更新周围的和自身的改变因子
        pm.changeEffectFactors(2f);
        //pm.attributes.effectFactors += 2;
    }
}
