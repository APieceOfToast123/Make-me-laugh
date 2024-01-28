using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件处理类
/// </summary>
public static class EventManager
{
    public static Action<PaientManager> AddSettledOne;
    public static Action FirstOneSettled;
    //public static Func<Transform, float, PaientManager[]> EffectCheck;
    public static Action CheckEffect;
    public static Action Charge;
    public static Action<PaientManager> AddDoctor;
    public static Action<PaientManager> DeleteSettledOne;
    public static Action<bool> TrySettlePatient;
    public static Func<List<GameObject>> GetBeds;
    
    public static void CallAddSettledOne(PaientManager settledOne)
    {
        AddSettledOne?.Invoke(settledOne);
    }
    public static void CallFirstOneSetteld()
    {
        FirstOneSettled?.Invoke();
    }

    // public static void CallAddDoctor(PaientManager treatingTarget)
    // {
    //     AddDoctor?.Invoke(treatingTarget);
    // }

    // public static PaientManager[] CallEffectCheck(Transform center, float radius)
    // {
    //     return EffectCheck?.Invoke(center, radius);
    // }
    
    public static void CallCheckEffect()
    {
        CheckEffect?.Invoke();
    }

    public static void CallDeleteSettledOne(PaientManager settledOne)
    {
        DeleteSettledOne?.Invoke(settledOne);
    }

    public static void CallCharge()
    {
        Charge?.Invoke();
    }
    public static void CallTrySettlePatient(bool isHit)
    {
        TrySettlePatient?.Invoke(isHit);
    }

    public static List<GameObject> CallGetBeds()
    {
        return GetBeds?.Invoke();
    }
}