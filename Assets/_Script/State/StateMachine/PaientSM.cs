using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaientSM : StateMachine
{
    /// <summary>
    /// 继承自父类的构造函数
    /// </summary>
    /// <param name="ownGo"></param>
    public PaientSM(GameObject ownGo) : base(ownGo)
    {
        Debug.Log(ownGo.name);
    }
}
