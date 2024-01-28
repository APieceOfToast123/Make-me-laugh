using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwitchVFX : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private PaientManager pm;

    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        pm = GetComponent<PaientManager>();

        // 添加空值检查
        if (particleSystem == null || pm == null)
        {
            Debug.LogError("Missing required components");
            return;
        }
    }

    void Update()
    {
        // 根据属性值切换粒子系统大小和颜色
        float newSize = 0.0f;
        Color newColor = Color.white; // 默认颜色为白色

        // 根据属性值切换图片
        if (pm.patientAttributes.mood > 70)
        {
            newSize = pm.patientAttributes.laughRadius * 2;
            newColor = Color.magenta; // 当 mood 大于 70 时设置颜色为粉色
        }
        else if (pm.patientAttributes.mood > 40)
        {
            newSize = 0.0f;
        }
        else
        {
            newSize = pm.patientAttributes.cryRadius * 2;
            newColor = Color.blue;
        }

        // 设置粒子系统的大小和颜色
        var mainModule = particleSystem.main;
        mainModule.startSize = newSize;
        mainModule.startColor = newColor;
    }
}
