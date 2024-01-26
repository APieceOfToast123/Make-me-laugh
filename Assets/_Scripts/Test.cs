using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float radius = 5f; // 检测半径
    public LayerMask layerMask; // 检测的层
    public float mood;

    void Start()
    {
        mood = 50;
        InvokeRepeating("ApplyEffect", 0f, 2f); // 每2秒执行一次
    }

    void ApplyEffect()
    {
        print("Effect");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Test>() != null)
            {
                mood -= 10;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // 在编辑器中显示效果范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
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
        GUI.Label(new Rect(screenPosition.x - 50, screenPosition.y - 25, 100, 50), "Mood: " + mood);
    }
    
    
}
