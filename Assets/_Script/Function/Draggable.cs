using System.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool IsDragging { get; private set; }

    private void OnMouseDown()
    {
        //拖动开始
        IsDragging = true;
        StartCoroutine(DragObject());
    }

    private IEnumerator DragObject()
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float enter;
        Plane plane = new Plane(Vector3.up, transform.position.y); // 创建一个与 Y 轴平行的平面

        if (plane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter); // 鼠标射线与平面的交点

            Vector3 offset = transform.position - hitPoint; // 计算偏移量

            while (Input.GetMouseButton(0))
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out enter))
                {
                    hitPoint = ray.GetPoint(enter);

                    transform.position = hitPoint + offset; // 更新位置
                }

                yield return new WaitForFixedUpdate();
            }
            
            //拖动结束
            IsDragging = false;
        }
    }
}