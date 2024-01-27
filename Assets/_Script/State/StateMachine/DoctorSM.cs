using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorSM : StateMachine
{
    /// <summary>
    /// 继承自父类的构造函数
    /// </summary>
    /// <param name="ownGo"></param>
    public DoctorSM(GameObject ownGo) : base(ownGo)
    {
//        Debug.Log(ownGo.name);
    }
}
