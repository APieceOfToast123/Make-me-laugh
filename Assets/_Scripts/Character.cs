using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    // 角色的属性
    public float influenceRadius = 5f;

    // 更新方法，用于检测鼠标事件
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 检测鼠标点击
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // 开始拖动 
                StartCoroutine(Dragging());
                print("1");
            }
        }
    }

    // 协程处理拖动逻辑
    
    IEnumerator Dragging()
    {
        while (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
            yield return null;
        }
        // 放置后影响其他角色
        InfluenceOtherCharacters();
    }

    // 影响其他角色的逻辑
    void InfluenceOtherCharacters()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, influenceRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.gameObject.GetComponent<Character>() != null)
            {
                // 在这里实现对其他角色的影响
                // 例如：hitCollider.gameObject.GetComponent<Character>().SomeMethod();
            }
        }
    }

    // 用于绘制影响半径的辅助视觉（仅在编辑器中显示）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, influenceRadius);
    }
}