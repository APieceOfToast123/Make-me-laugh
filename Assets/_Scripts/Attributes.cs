using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Attributes
{
    public float laughRadius;
    public float cryRadius;
    public float mood;
    public float effectFactors;
    public float tickEffectAmount;

    public Attributes()
    {
        laughRadius = 4.0f;
        cryRadius = 5.0f;
        mood = 0f;
        effectFactors = 1;
        tickEffectAmount = 0;
    }
}
