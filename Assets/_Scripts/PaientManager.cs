using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class PaientManager : MonoBehaviour
{
    public Attributes attributes;
    public LayerMask layermask;
    private PaientSM sm;

    private void Awake()
    {
        attributes = new Attributes();

        //新建状态机
        sm = new PaientSM(this.gameObject);
        
        //为状态机添加状态，第一个被添加的状态被视为初始状态
        sm.AddState(StateID.Seletable,new SSelectable(sm));
        sm.AddState(StateID.Laugh,new SLaugh(sm));
        
    }

    private void Update()
    {
        if (sm.CurrentState != null)
            sm.Update();
    }

    public void changeMood(float changeMood)
    {
        attributes.mood += changeMood;
    }

    public void changeEffectFactors(float changeValue)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attributes.laughRadius, layermask);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Test>() != null)
            {
                attributes.effectFactors += changeValue;
            }
        }
    }

    void OnGUI()
    {
        GUI.color = Color.black;
        // 将角色的世界坐标转换为屏幕坐标
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        // 设置 GUI 文本的位置
        screenPosition.y = Screen.height - screenPosition.y; // 注意 Unity 屏幕坐标的 Y 轴是从下往上的
        screenPosition.y += 10; // 在角色的正上方 10 像素处显示

        // 绘制 GUI 文本
        GUI.Label(new Rect(screenPosition.x - 50, screenPosition.y - 25, 100, 50), "Mood: " + attributes.mood);
    }
}
